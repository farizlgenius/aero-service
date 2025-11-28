using HIDAeroService.DTO;
using HIDAeroService.DTO.Auth;
using HIDAeroService.DTO.Token;
using HIDAeroService.Helpers;
using HIDAeroService.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

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

        [HttpPost("logout")]
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

            var userJson = User.FindFirst("user")?.Value ?? "";
            var user = string.IsNullOrEmpty(userJson) ? new Users
            {
                Title = "",
                Firstname = "",
                Middlename = "",
                Lastname = "",
                Email = ""
            } : JsonSerializer.Deserialize<Users>(userJson);

            var locJson = User.FindFirst("location")?.Value ?? "";
            var loc = string.IsNullOrEmpty(locJson) ? [] : JsonSerializer.Deserialize<List<short>>(locJson);
            var roleJson = User.FindFirst("rol")?.Value ?? "";
            var rol = string.IsNullOrEmpty(roleJson) ? new Role
            {
                Features = [],
                RoleName = "",
                RoleNo = 0
            } : JsonSerializer.Deserialize<DTO.Token.Role>(roleJson);

            var info = new TokenInfo
            {
                User = user,
                Locations = loc,
                Role = rol
            };
            var dto = new TokenDetail
            {
                Auth = true,
                //Info = info,
            };
            return Ok(
                new
                {
                    Auth = true,
                    User = new
                    {
                        Id = userId,
                        name,
                        Info = user,
                        Location = loc,
                        Role = rol
                    }
                }
                );
        }


    }
}

