
using System.Security.Claims;
using Aero.Application.DTOs;

namespace Aero.Application.Interface
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
