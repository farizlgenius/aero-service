using HIDAeroService.DTO;
using HIDAeroService.DTO.Auth;
using HIDAeroService.DTO.Token;
using HIDAeroService.Helpers;
using HIDAeroService.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace HIDAeroService.Controllers.V1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        [HttpPost("login")]
        public async Task<ActionResult<ResponseDto<TokenDto>>> Login([FromBody] LoginDto model)
        {
            var res = await authService.LoginAsync(model,Request,Response);
            return Ok(res);
        }

        [HttpPost("refresh")]
        public async Task<ActionResult<ResponseDto<TokenDto>>> Refresh()
        {
            // HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown"
            var res = await authService.RefreshAsync(Request,Response);
            return Ok(res);

        }

        [HttpPost("revoke")]
        public async Task<ActionResult<ResponseDto<bool>>> Revoke()
        {

            var res = await authService.RevokeAsync(Request,Response);
            return Ok(res);

        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("me")]
        public IActionResult Me()
        {
            var userId = User.FindFirst("sub")?.Value ?? User.Identity?.Name ?? "unknown";
            var name = User.Identity?.Name;
            return Ok(new { auth = true, user = new {id=userId,name}});
        }


    }
}

