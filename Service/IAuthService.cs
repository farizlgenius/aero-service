using HIDAeroService.DTO;
using HIDAeroService.DTO.Auth;
using HIDAeroService.DTO.Token;
using HIDAeroService.Entity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HIDAeroService.Service
{
    public interface IAuthService
    {
        Task<ResponseDto<TokenDto>> LoginAsync(LoginDto login,HttpRequest request,HttpResponse response);
        Task<ResponseDto<TokenDto>> RefreshAsync(HttpRequest request,HttpResponse response);
        Task<ResponseDto<bool>> RevokeAsync(HttpRequest request,HttpResponse response);
        ResponseDto<TokenDetail> Me(ClaimsPrincipal User);
        bool ValidateLogin(Operator model,string Password);
    }
}
