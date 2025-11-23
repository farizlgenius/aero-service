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
    public sealed class JwtTokenService : IJwtTokenService
    {
        private readonly string _secret;
        private readonly string _issuer;
        private readonly string _audience;
        private readonly int _minutes;
        private readonly AppDbContext _context;
        public JwtTokenService(IConfiguration cfg, AppDbContext db)
        {
            _context = db;
            _secret = cfg["Jwt:Secret"]!;
            _issuer = cfg["Jwt:Issuer"]!;
            _audience = cfg["Jwt:Audience"]!;
            _minutes = int.Parse(cfg["Jwt:AccessTokenMinutes"] ?? "5");
        }

        public string CreateAccessToken(string userId, string username)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var now = DateTime.UtcNow;

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub,userId),
                new Claim(ClaimTypes.Name,username),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                claims: claims,
                notBefore: now,
                expires: now.AddMinutes(_minutes),
                signingCredentials: creds
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
