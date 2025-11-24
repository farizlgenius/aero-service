using HIDAeroService.Data;
using HIDAeroService.DTO;
using HIDAeroService.DTO.Auth;
using HIDAeroService.DTO.Operator;
using HIDAeroService.DTO.Token;
using HIDAeroService.Entity;
using HIDAeroService.Helpers;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Asn1.Ocsp;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace HIDAeroService.Service.Impl
{
    public class AuthService(IConfiguration configuration, AppDbContext context, IJwtTokenService tokenService, IRefreshTokenStore refresh) : IAuthService
    {
        private readonly TimeSpan _refreshTtl = TimeSpan.FromDays(30);
        private readonly TimeSpan _cookieExpiry = TimeSpan.FromDays(30);
        public bool ValidateLogin(Operator user, string Password)
        {
            return EncryptHelper.VerifyPassword(Password, user.Password);
        }

        public async Task<ResponseDto<TokenDto>> LoginAsync(LoginDto model, HttpRequest request, HttpResponse response)
        {
            var user = await context.Operators
                .AsNoTracking()
                .Include(x => x.Location)
                 .Include(x => x.Role)
                .ThenInclude(x => x.FeatureRoles)
                .Where(x => x.Username == model.Username)
                .OrderBy(x => x.ComponentId)
                .FirstOrDefaultAsync();

            if (user is null) return ResponseHelper.NotFoundBuilder<TokenDto>(["User not found."]);

            // TODO: Replace with real user validation (DB, hashed passwords)
            if (!ValidateLogin(user, model.Password))
                return ResponseHelper.Unauthorize<TokenDto>(["Password incorrect."]);


            var accessToken = tokenService.CreateAccessToken(user.UserId, model.Username,user.Location,user.Role,user.Email,user.Title,user.FirstName,user.MiddleName,user.LastName);

            // create random refresh token and store hashed in redis + audit in DB
            var rawRefresh = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
            
            await refresh.StoreTokenAsync(rawRefresh, user.UserId,user.Username, _refreshTtl, info: request.HttpContext.Connection.RemoteIpAddress.ToString());

            // set HttpOnly cookies (path limited to auth endpoint)
            response.Cookies.Append("refresh_token", rawRefresh, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Path = "/api/v1/Auth",
                Expires = DateTimeOffset.UtcNow.Add(_cookieExpiry)
            });

            var dto = new TokenDto
            {
                TimeStamp = DateTime.UtcNow,
                AccessToken = accessToken,
            };

            return ResponseHelper.SuccessBuilder<TokenDto>(dto);
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

        public async Task<ResponseDto<TokenDto>> RefreshAsync(HttpRequest request, HttpResponse response)
        {
            if (!request.Cookies.TryGetValue("refresh_token", out var oldRaw)) return ResponseHelper.Unauthorize<TokenDto>(["no refresh token"]);

            var rec = await refresh.GetByRawTokenAsync(oldRaw);
            if (rec == null || rec.ExpireAt < DateTime.UtcNow)
            {
                // token invalid expire
                if (rec != null) await refresh.RevokeTokenAsync(oldRaw);
                return ResponseHelper.Unauthorize<TokenDto>(["Invalid refresh token"]);
            }

            var user = await context.Operators
                .AsNoTracking()
                .Include(x => x.Location)
                .Include(x => x.Role)
                .ThenInclude(x => x.FeatureRoles)
                .Where(x => x.UserId == rec.UserId)
                .FirstOrDefaultAsync();

            if (String.IsNullOrEmpty(user.Username)) return ResponseHelper.NotFoundBuilder<TokenDto>(["can not automatic create token username with specific userid not found"]);

            // rotate token automatically: create new raw token and swap in redis
            var newRaw = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
            try
            {
                await refresh.RotateTokenAtomicAsync(oldRaw, newRaw, rec.UserId, rec.Username,_refreshTtl, request.HttpContext.Connection.RemoteIpAddress.ToString());
            }
            catch (InvalidOperationException ex)
            {
                return ResponseHelper.Unauthorize<TokenDto>(["Token reuse detected"]);
            }



            // issue new access token
            var accessToken = tokenService.CreateAccessToken(rec.UserId, user.Username,user.Location,user.Role,user.Email,user.Title,user.FirstName,user.MiddleName,user.LastName);

            // set rotate cookie
            response.Cookies.Append("refresh_token", newRaw, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Path = "/api/v1/Auth",
                Expires = DateTime.UtcNow.Add(_cookieExpiry)
            });

            var dto = new TokenDto
            {
                AccessToken = accessToken,
                TimeStamp = DateTime.UtcNow,
            };
            return ResponseHelper.SuccessBuilder<TokenDto>(dto);
            
        }

        public async Task<ResponseDto<bool>> RevokeAsync(HttpRequest request,HttpResponse response)
        {
            if(request.Cookies.TryGetValue("refresh_token",out var raw))
            {
                await refresh.RevokeTokenAsync(raw);
            }

            response.Cookies.Delete("refresh_token", new CookieOptions
            {
                HttpOnly=true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Path="/api/v1/Auth",
            });

            return ResponseHelper.SuccessBuilder<bool>(true);
        }

        public ResponseDto<TokenDetail> Me(ClaimsPrincipal User)
        {
            //var userId = User.FindFirst("sub")?.Value ?? User.Identity?.Name ?? "unknown";
            //var name = User.Identity?.Name;
            //var ujson = User.FindFirst("user")?.Value;
            //var user = JsonSerializer.Deserialize<Users>(ujson);
            //var ljson = User.FindFirst("location")?.Value;
            //var loc = JsonSerializer.Deserialize<DTO.Token.Location>(ljson);
            //var rjson = User.FindFirst("role")?.Value;
            //var rol = JsonSerializer.Deserialize<DTO.Token.Role>(rjson);
            //var info = new TokenInfo(user, loc, rol);
            //var dto = new TokenDetail(true, info);
            return ResponseHelper.SuccessBuilder<TokenDetail>(null);
        }
    }
}
