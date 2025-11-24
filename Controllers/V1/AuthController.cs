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

        //[Authorize(AuthenticationSchemes = "Bearer")]
        //[HttpGet("me")]
        //public ActionResult<ResponseDto<TokenDetail>> Me()
        //{
        //    var userId = User.FindFirst("sub")?.Value ?? User.Identity?.Name ?? "unknown";
        //    var name = User.Identity?.Name;

        //    var userJson = User.FindFirst("user")?.Value ?? "";
        //    var user = JsonSerializer.Deserialize<Users>(userJson);

        //    var locJson = User.FindFirst("location")?.Value ?? "";
        //    var loc = JsonSerializer.Deserialize<DTO.Token.Location>(locJson);

        //    var roleJson = User.FindFirst("role")?.Value ?? "";
        //    var rol = JsonSerializer.Deserialize<DTO.Token.Role>(roleJson);

        //    var info = new TokenInfo
        //    {
        //        User = user ?? new Users { },
        //        Location = loc ?? new Location { },
        //        Role = rol ?? new Role { }
        //    };
        //    var dto = new TokenDetail
        //    {
        //        Auth = true,
        //        Info = info,
        //    };
        //    return Ok(ResponseHelper.SuccessBuilder<TokenDetail>(dto));
        //}

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("me")]
        public ActionResult<ResponseDto<TokenDetail>> Me()
        {
            var userId = User.FindFirst("sub")?.Value ?? User.Identity?.Name ?? "unknown";

            // Generic helper local function for safe JSON deserialization from a claim
            T? SafeDeserializeClaim<T>(string claimType) where T : class
            {
                var json = User.FindFirst(claimType)?.Value;
                if (string.IsNullOrWhiteSpace(json))
                    return null;

                try
                {
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    return JsonSerializer.Deserialize<T>(json, options);
                }
                catch (JsonException)
                {
                    // optionally log the bad claim value here
                    return null;
                }
            }

            var user = SafeDeserializeClaim<Users>("user") ?? new Users();
            var loc = SafeDeserializeClaim<DTO.Token.Location>("location") ?? new Location();
            var rol = SafeDeserializeClaim<DTO.Token.Role>("role") ?? new Role();

            var info = new TokenInfo
            {
                User = user,
                Location = loc,
                Role = rol
            };

            var dto = new TokenDetail
            {
                Auth = true,
                Info = info,
            };

            return Ok(ResponseHelper.SuccessBuilder<TokenDetail>(dto));
        }


    }
}

