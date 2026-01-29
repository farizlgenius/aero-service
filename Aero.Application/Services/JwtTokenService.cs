using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Aero.Application.Interface;

namespace Aero.Application.Services
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

        public string CreateAccessToken(string userId, string username, List<Location> location, Role rol,string title,string email, string firstname, string middlename, string lastname)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var now = DateTime.UtcNow;
            var users = new
            {
                Title = title ?? "",
                Firstname = firstname ?? "",
                Middlename = middlename ?? "",
                Lastname = lastname ?? "",
                Email = email ?? "",
            };
            var locations = location.Select(x => x.component_id).ToList();
            var roles = new
            {
                RoleNo = rol.component_id,
                RoleName = rol.name,
                Features = rol.feature_roles
                .Where(x => x.is_allow == true)
                .Select(x => x.feature_id)
                .ToList()
            };


            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub,userId),
                new Claim(ClaimTypes.Name,username),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),

                // Custom Claims
                new Claim("user",JsonSerializer.Serialize(users)),
                new Claim("location",JsonSerializer.Serialize(locations)),
                new Claim("rol",JsonSerializer.Serialize(roles))
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
