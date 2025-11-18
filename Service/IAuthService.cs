using HIDAeroService.DTO.Auth;
using Microsoft.AspNetCore.Mvc;

namespace HIDAeroService.Service
{
    public interface IAuthService
    {
        string Login(LoginDto login);
    }
}
