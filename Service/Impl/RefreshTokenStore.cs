using HIDAeroService.Data;
using HIDAeroService.Entity;
using HIDAeroService.Helpers;
using HIDAeroService.Model;
using StackExchange.Redis;
using System.Text.Json;

namespace HIDAeroService.Service.Impl
{
    public sealed class RefreshTokenStore : IRefreshTokenStore
    {
        private readonly IDatabase _redis;
        private readonly AppDbContext _context;
        private readonly JsonSerializerOptions jopts = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        private const string RotateLua = @"
            if redis.call('EXISTS', KEYS[1]) == 1 then
               redis.call('DEL', KEYS[1])
               redis.call('SET', KEYS[2], ARGV[1], 'PX', ARGV[2])
               return 1
             else
               return 0
             end
        ";

        public RefreshTokenStore(AppDbContext context, IConnectionMultiplexer multiplexer)
        {
            _context = context;
            _redis = multiplexer.GetDatabase();
        }


        private static string RedisKey(string hashed) => $"refresh:{hashed}";

        public async Task StoreTokenAsync(string rawToken,string userId,string username, TimeSpan ttl, string? info = null)
        {
            var hashed = EncryptHelper.Hash(rawToken);
            var payload = new { userId,username,createdAt = DateTime.UtcNow,expiresAt = DateTime.UtcNow.Add(ttl) };
            var json = JsonSerializer.Serialize(payload,jopts);

            await _redis.StringSetAsync(RedisKey(hashed),json,ttl);

            // write audit row
            var audit = new RefreshTokenAudit
            {
                HashedToken = hashed,
                UserId = userId,
                Username = username,
                Action = "create",
                Info = info,
                CreatedDate = DateTime.Now,
            };
            _context.RefreshTokenAudits.Add(audit);
            await _context.SaveChangesAsync();
        }

        public async Task<RefreshTokenRecord?> GetByRawTokenAsync(string rawToken)
        {
            var hashed = EncryptHelper.Hash(rawToken);
            var val = await _redis.StringGetAsync(RedisKey(hashed));
            if(val.IsNullOrEmpty) return null;
            var doc = JsonSerializer.Deserialize<JsonElement>(val,jopts);
            var userId = doc.GetProperty("userId").GetString();
            var userName = doc.GetProperty("username").GetString();
            var expiresAt = doc.GetProperty("expiresAt").GetDateTime();
            return new RefreshTokenRecord(hashed,userId,userName,expiresAt);
        }

        public async Task RotateTokenAtomicAsync(string oldRawToken, string newRawToken, string userId,string username, TimeSpan ttl, string? info = null)
        {
            var oldHashed = EncryptHelper.Hash(oldRawToken);
            var newHashed = EncryptHelper.Hash(newRawToken);

            var payload = new {userId,createdAt = DateTime.UtcNow,expiresAt = DateTime.UtcNow.Add(ttl)};
            var json = JsonSerializer.Serialize(payload, jopts);

            // execute LUA script against Redis
            var result = (int)await _redis.ScriptEvaluateAsync(RotateLua,
                new RedisKey[] {RedisKey(oldHashed),RedisKey(newHashed)},
                new RedisValue[] {json,(long)ttl.TotalMilliseconds}
                );

            if(result == 1)
            {
                // audit rotation in DB
                _context.RefreshTokenAudits.Add(
                    new RefreshTokenAudit 
                    {
                        HashedToken = newHashed,
                        UserId = userId,
                        Username = username,
                        Action = "rotate",
                        Info = info,
                        CreatedDate = DateTime.Now,
                    }
                );
                await _context.SaveChangesAsync();
            }
            else
            {
                // rotation failed: old token didn't exist (possible reuse or already rotated)
                throw new InvalidOperationException("rotation failed: old token missing");
            }
        }

        public async Task RevokeTokenAsync(string rawToken)
        {
            var hashed = EncryptHelper.Hash(rawToken);
            await _redis.KeyDeleteAsync(RedisKey(hashed));

            _context.RefreshTokenAudits.Add(
                new RefreshTokenAudit 
                {
                    HashedToken = hashed,
                    UserId = "unknown",
                    Username = "unknown",
                    Action = "revoke",
                    CreatedDate= DateTime.Now,
                }    
            );
            await _context.SaveChangesAsync();
        }




    }
}
