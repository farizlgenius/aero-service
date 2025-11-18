using HIDAeroService.Entity;
using System.Security.Claims;

namespace HIDAeroService.Service
{
    public interface ITokenService
    {
        string GenerateAccessToken(string username, IEnumerable<Claim>? additionalClaims = null);
        RefreshToken GenerateRefreshToken(string username, string ip);
        Task SaveRefreshTokenAsync(RefreshToken token);
        Task<RefreshToken?> GetRefreshTokenAsync(string token);
        Task<RefreshToken?> RotateRefreshTokenAsync(RefreshToken existing, string ip);
        Task RevokeRefreshTokenAsync(RefreshToken token, string? reason = null);
    }
}
