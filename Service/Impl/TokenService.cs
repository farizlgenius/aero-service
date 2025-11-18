using HIDAeroService.Data;
using HIDAeroService.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace HIDAeroService.Service.Impl
{
    public sealed class TokenService : ITokenService
    {
        private readonly IConfiguration _config;
        private readonly AppDbContext _db;
        private readonly byte[] _signingKey;
        private readonly string _issuer;
        private readonly string _audience;
        private readonly int _accessTokenMinutes;
        private readonly int _refreshTokenDays;
        public TokenService(IConfiguration config, AppDbContext db)
        {
            _config = config;
            _db = db;

            // read config (set Jwt:Key via env var or Key Vault in production)
            var key = _config["Jwt:Key"] ?? throw new Exception("Jwt:Key not configured");
            _signingKey = Encoding.UTF8.GetBytes(key);

            _issuer = _config["Jwt:Issuer"] ?? "my-issuer";
            _audience = _config["Jwt:Audience"] ?? "my-audience";
            _accessTokenMinutes = int.Parse(_config["Jwt:ExpireMinutes"] ?? "15");
            _refreshTokenDays = int.Parse(_config["Jwt:RefreshDays"] ?? "7");
        }

        public string GenerateAccessToken(string username, IEnumerable<Claim>? additionalClaims = null)
        {
            var now = DateTime.UtcNow;
            var jti = Guid.NewGuid().ToString();

            var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, username),
            new Claim(JwtRegisteredClaimNames.Jti, jti),
            new Claim(ClaimTypes.Name, username)
        };

            if (additionalClaims != null) claims.AddRange(additionalClaims);

            var creds = new SigningCredentials(new SymmetricSecurityKey(_signingKey), SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                claims: claims,
                notBefore: now,
                expires: now.AddMinutes(_accessTokenMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public RefreshToken GenerateRefreshToken(string username, string ip)
        {
            var randomBytes = RandomNumberGenerator.GetBytes(64);
            var token = Convert.ToBase64String(randomBytes);
            return new RefreshToken
            {
                Token = token,
                UserName = username,
                JwtId = null, // optional: set matching jti when issuing token
                CreatedAt = DateTime.UtcNow,
                CreatedByIp = ip,
                ExpiresAt = DateTime.UtcNow.AddDays(_refreshTokenDays),
                IsRevoked = false
            };
        }

        public async Task SaveRefreshTokenAsync(RefreshToken token)
        {
            _db.RefreshTokens.Add(token);
            await _db.SaveChangesAsync();
        }

        public async Task<RefreshToken?> GetRefreshTokenAsync(string token)
        {
            return await _db.RefreshTokens.FirstOrDefaultAsync(t => t.Token == token);
        }

        // Rotate: revoke existing and issue a new one, linking them
        public async Task<RefreshToken?> RotateRefreshTokenAsync(RefreshToken existing, string ip)
        {
            if (existing == null || !existing.IsActive) return null;

            // create new
            var newToken = GenerateRefreshToken(existing.UserName, ip);
            newToken.ReplacedByToken = null;

            // revoke old, link to new
            existing.IsRevoked = true;
            existing.RevokedAt = DateTime.UtcNow;
            existing.ReplacedByToken = newToken.Token;

            _db.RefreshTokens.Update(existing);
            _db.RefreshTokens.Add(newToken);

            await _db.SaveChangesAsync();
            return newToken;
        }

        public async Task RevokeRefreshTokenAsync(RefreshToken token, string? reason = null)
        {
            if (token == null || token.IsRevoked) return;

            token.IsRevoked = true;
            token.RevokedAt = DateTime.UtcNow;
            _db.RefreshTokens.Update(token);
            await _db.SaveChangesAsync();
        }


    }
}
