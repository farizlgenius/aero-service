
namespace Aero.Application.Interface
{
    public interface IRefreshTokenStore
    {
        Task StoreTokenAsync(string rawToken, string userId,string username, TimeSpan ttl, string? info = null);
        Task<RefreshTokenRecord?> GetByRawTokenAsync(string rawToken,HttpRequest request);
        Task RotateTokenAtomicAsync(string oldRawToken, string newRawToken, string userId,string username, TimeSpan ttl, string? info = null);
        Task RevokeTokenAsync(string rawToken);
    }
}
