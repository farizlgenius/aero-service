using HIDAeroService.DTO.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HIDAeroService.Service.Impl
{
    public class AuthService(IConfiguration configuration) : IAuthService
    {
        public string Login(LoginDto login)
        {
            var token = GenerateJwtToken(login.Username, isAdmin: login.Username == "admin");
            return token;
        }

        private string GenerateJwtToken(string username, bool isAdmin)
        {
            var jwt = configuration.GetSection("Jwt");
            var key = jwt["Key"];
            var issuer = jwt["Issuer"];
            var audience = jwt["Audience"];
            var expireMinutes = int.Parse(jwt["ExpireMinutes"]);

            var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, username),
            new Claim(ClaimTypes.Name, username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            // custom claims:
            new Claim("role", isAdmin ? "Admin" : "User")
        };

            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var creds = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expireMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
