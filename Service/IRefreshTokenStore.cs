using HIDAeroService.Model;

namespace HIDAeroService.Service
{
    public interface IRefreshTokenStore
    {
        Task StoreTokenAsync(string rawToken, string userId, TimeSpan ttl, string? info = null);
        Task<RefreshTokenRecord?> GetByRawTokenAsync(string rawToken);
        Task RotateTokenAtomicAsync(string oldRawToken, string newRawToken, string userId, TimeSpan ttl, string? info = null);
        Task RevokeTokenAsync(string rawToken);
    }
}
