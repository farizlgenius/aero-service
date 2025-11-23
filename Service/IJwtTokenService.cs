using HIDAeroService.Entity;
using System.Security.Claims;

namespace HIDAeroService.Service
{
    public interface IJwtTokenService
    {
        string CreateAccessToken(string userId, string username);

    }
}
