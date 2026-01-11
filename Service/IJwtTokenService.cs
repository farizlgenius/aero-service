using AeroService.Entity;
using System.Security.Claims;

namespace AeroService.Service
{
    public interface IJwtTokenService
    {
        string CreateAccessToken(string userId, string username,List<Location> locations,Role role,string email,string title,string firstname,string middlename,string lastname);

    }
}
