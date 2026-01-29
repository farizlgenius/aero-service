
namespace Aero.Application.Services
{
    public sealed class RefreshTokenStore(AppDbContext context) : IRefreshTokenStore
    {
        private readonly JsonSerializerOptions jopts = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };



        public async Task StoreTokenAsync(string rawToken,string userId,string username, TimeSpan ttl, string? info = null)
        {
            var hashed = EncryptHelper.Hash(rawToken);

            // write audit row
            var audit = new RefreshToken
            {
                hashed_token = hashed,
                user_id = userId,
                user_name = username,
                action = "create",
                info = info,
                created_date = DateTime.UtcNow,
                updated_date = DateTime.UtcNow,
                expire_date = DateTime.UtcNow.Add(ttl)
            };
            await context.refresh_token.AddAsync(audit);
            await context.SaveChangesAsync();
        }

        public async Task<RefreshTokenRecord?> GetByRawTokenAsync(string rawToken,HttpRequest request)
        {
            var hashed = EncryptHelper.Hash(rawToken);
            var refresh = await context.refresh_token
                .AsNoTracking()
                .Where(x => x.hashed_token.Equals(hashed))
                .FirstOrDefaultAsync();

            if (refresh is null) return null;
            if (refresh.action.Equals("revoke")) return null;

            var userId = refresh.user_id;
            var userName = refresh.user_id;
            var expiresAt = refresh.expire_date;
            return new RefreshTokenRecord(hashed, userId, userName, expiresAt);

        }

        public async Task RotateTokenAtomicAsync(string oldRawToken, string newRawToken, string userId,string username, TimeSpan ttl, string? info = null)
        {
            var newHashed = EncryptHelper.Hash(newRawToken);


            // audit rotation in DB
            context.refresh_token.Add(
                new RefreshToken
                {
                    hashed_token = newHashed,
                    user_id = userId,
                    user_name = username,
                    action = "rotate",
                    info = info,
                    created_date = DateTime.UtcNow,
                    expire_date = DateTime.UtcNow.Add(ttl)
                }
            );
            await context.SaveChangesAsync();
        }

        public async Task RevokeTokenAsync(string rawToken)
        {
            var hashed = EncryptHelper.Hash(rawToken);

            context.refresh_token.Add(
                new RefreshToken 
                {
                    hashed_token = hashed,
                    user_id = "unknown",
                    user_name = "unknown",
                    action = "revoke",
                    created_date= DateTime.UtcNow,
                }    
            );
            await context.SaveChangesAsync();
        }




    }
}
