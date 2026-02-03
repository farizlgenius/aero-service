using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Aero.Application.DTOs;
using Aero.Application.Helpers;
using Aero.Application.Interface;
using Aero.Application.Interfaces;
using Aero.Domain.Entities;
using Aero.Domain.Helpers;
using Aero.Domain.Interface;
using Aero.Infrastructure.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Aero.Application.Services
{
    public class AuthService(IOptions<JwtSettings> settings, IQOperatorRepository qOper,IQRoleRepository qRole,IAuthRepository auth) : IAuthService
    {
        private readonly string _secret = settings.Value.Secret;
        private readonly string _issuer = settings.Value.Issuer;
        private readonly string _audience = settings.Value.Audience;
        private readonly int _minutes = settings.Value.AccessTokenMinute;
        private readonly TimeSpan _refreshTtl = TimeSpan.FromHours(3);
        

        private readonly JsonSerializerOptions jopts = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        public bool ValidateLogin(string StoreHashed, string Password)
        {
            return EncryptHelper.VerifyPassword(Password, StoreHashed);
        }

        public async Task<ResponseDto<TokenDtoWithRefresh>> LoginAsync(LoginDto model,string ip)
        {

            var user = await qOper.GetByUsernameAsync(model.Username);
            var hash = await qOper.GetPasswordByUsername(model.Username);

            if (user is null) return ResponseHelper.NotFoundBuilder<TokenDtoWithRefresh>(["User not found."]);

            var role = await qRole.GetByComponentIdAsync(user.RoleId);

            

            // TODO: Replace with real user validation (DB, hashed passwords)
            if (!ValidateLogin(hash, model.Password))
                return ResponseHelper.Unauthorize<TokenDtoWithRefresh>(["password incorrect."]);

            var accessToken = CreateAccessToken(user,role);

            // create random refresh token and store hashed in redis + audit in DB
            var rawRefresh = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

            await StoreTokenAsync(rawRefresh, user.UserId, user.Username, _refreshTtl,info:ip);

            var dto = new TokenDtoWithRefresh
            {
                TimeStamp = DateTime.UtcNow,
                AccessToken = accessToken,
                RefreshToken = rawRefresh,
                ExpireInMinute = (int)_refreshTtl.TotalMinutes
            };

            return ResponseHelper.SuccessBuilder<TokenDtoWithRefresh>(dto);
        }


        public async Task<ResponseDto<TokenDtoWithRefresh>> RefreshAsync(string oldRaw,string ip)
        {
          


            var rec = await GetByRawTokenAsync(oldRaw);

            if (rec == null || rec.ExpireAt < DateTime.UtcNow)
            {
                // token invalid expire
                if (rec != null) await RevokeTokenAsync(oldRaw);
                return ResponseHelper.Unauthorize<TokenDtoWithRefresh>(["Invalid refresh token"]);
            }

            var user = await qOper.GetByUsernameAsync(rec.Username);
            var role = await qRole.GetByComponentIdAsync(user.RoleId);

            if (user is null) return ResponseHelper.NotFoundBuilder<TokenDtoWithRefresh>(["User not found."]);

            if (String.IsNullOrEmpty(user.Username)) return ResponseHelper.NotFoundBuilder<TokenDtoWithRefresh>(["Can not automatic create token username with specific userid not found"]);

            // rotate token automatically: create new raw token and swap in redis
            var newRaw = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
            try
            {
                await RotateTokenAtomicAsync(oldRaw, newRaw, rec.UserId, rec.Username, _refreshTtl, ip);
            }
            catch (InvalidOperationException ex)
            {
                return ResponseHelper.Unauthorize<TokenDtoWithRefresh>(["Token reuse detected"]);
            }



            // issue new access token
            var accessToken = CreateAccessToken(user,role);


            var dto = new TokenDtoWithRefresh
            {
                AccessToken = accessToken,
                TimeStamp = DateTime.UtcNow,
                RefreshToken = newRaw,
                ExpireInMinute = (int)_refreshTtl.TotalMinutes
            };
            return ResponseHelper.SuccessBuilder<TokenDtoWithRefresh>(dto);

        }

        public async Task<ResponseDto<bool>> RevokeAsync(string rawToken)
        {
            

            await RevokeTokenAsync(rawToken);

            return ResponseHelper.SuccessBuilder<bool>(true);
        }

        public ResponseDto<TokenDetail> Me(ClaimsPrincipal User)
        {
            //var userId = User.FindFirst("sub")?.value ?? User.Identity?.name ?? "unknown";
            //var name = User.Identity?.name;
            //var ujson = User.FindFirst("user")?.value;
            //var user = JsonSerializer.Deserialize<Users>(ujson);
            //var ljson = User.FindFirst("location")?.value;
            //var loc = JsonSerializer.Deserialize<DTO.Token.location>(ljson);
            //var rjson = User.FindFirst("role")?.value;
            //var rol = JsonSerializer.Deserialize<DTO.Token.role>(rjson);
            //var info = new TokenInfo(user, loc, rol);
            //var dto = new TokenDetail(true, info);
            return ResponseHelper.SuccessBuilder<TokenDetail>(null);
        }

        public string CreateAccessToken(OperatorDto user,RoleDto role)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var now = DateTime.UtcNow;
            var users = new
            {
                Title = user.Title ?? "",
                Firstname = user.FirstName ?? "",
                Middlename = user.MiddleName ?? "",
                Lastname = user.LastName ?? "",
                Email = user.Email ?? "",
            };
            var locations = user.LocationIds;
            var roles = new 
            {
                RoleNo = role.ComponentId,
                RoleName = role.Name,
                Features = role.Features.Select(x => x.ComponentId).ToList()
            };


            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub,user.UserId),
                new Claim(ClaimTypes.Name,user.Username),
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

        public async Task StoreTokenAsync(string rawToken,string userId,string username, TimeSpan ttl, string? info = null)
        {
            var hashed = EncryptHelper.Hash(rawToken);

            // write audit row
            var status = await auth.AddRefreshTokenAsync(hashed,userId,username,"",ttl);
            if(status <= 0) throw new Exception("Can't Save new refresh token to database.");
        }

        public async Task<RefreshTokenRecord> GetByRawTokenAsync(string rawToken)
        {
            var hashed = EncryptHelper.Hash(rawToken);
            var refresh = await auth.GetRefreshTokenByHashed(hashed);

            if (refresh is null) return null;
            if (refresh.Action.Equals("revoke")) return null;

            var userId = refresh.UserId;
            var userName = refresh.Username;
            var expiresAt = refresh.ExpireDate;
            return new RefreshTokenRecord(hashed, userId, userName, expiresAt);

        }

        public async Task RotateTokenAtomicAsync(string oldRawToken, string newRawToken, string userId,string username, TimeSpan ttl, string? info = null)
        {
            var newHashed = EncryptHelper.Hash(newRawToken);

            // audit rotation in DB
            var status = await auth.RotateRefreshTokenAsync(newHashed,userId,username,"",ttl);
            if(status <= 0) throw new Exception("Save new refresh token to database unsucccess.");
        }

        public async Task RevokeTokenAsync(string rawToken)
        {
            var hashed = EncryptHelper.Hash(rawToken);

            // audit rotation in DB
            var status = await auth.RevokeTokenAsync(hashed);
            if(status <= 0) throw new Exception("Revoke token unsuccess.");
        }
    }
}
