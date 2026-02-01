
using System.Security.Claims;
using Aero.Application.DTOs;
using Aero.Domain.Entities;

namespace Aero.Application.Interface
{
    public interface IAuthService
    {
        Task<ResponseDto<TokenDtoWithRefresh>> LoginAsync(LoginDto login,string ip);
        Task<ResponseDto<TokenDtoWithRefresh>> RefreshAsync(string oldRaw,string ip);
        Task<ResponseDto<bool>> RevokeAsync(string token);
        ResponseDto<TokenDetail> Me(ClaimsPrincipal User);
        bool ValidateLogin(string hashed,string Password);
         public string CreateAccessToken(OperatorDto user,RoleDto role);
         Task StoreTokenAsync(string rawToken, string userId,string username, TimeSpan ttl, string? info = null);
        Task<RefreshTokenRecord> GetByRawTokenAsync(string rawToken);
        Task RotateTokenAtomicAsync(string oldRawToken, string newRawToken, string userId,string username, TimeSpan ttl, string? info = null);
        Task RevokeTokenAsync(string rawToken);

    }
}
