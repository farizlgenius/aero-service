

using System.Security.Claims;
using System.Text.Json;
using Aero.Application.DTOs;
using Aero.Application.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aero.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthService auth,IOperatorService oper) : ControllerBase
    {
        private readonly TimeSpan _cookieExpiry = TimeSpan.FromHours(3);

        [HttpPost("login")]
        public async Task<ActionResult<ResponseDto<TokenDto>>> Login([FromForm] LoginDto model)
        {
            var res = await auth.LoginAsync(model, Request.HttpContext.Connection.RemoteIpAddress is null ? "" : Request.HttpContext.Connection.RemoteIpAddress.ToString());
            // set HttpOnly cookies (path limited to auth endpoint)
            if(res.data is not null)
            {
                Response.Cookies.Append("refresh_token", res.data.RefreshToken, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Path = "/api/Auth",
                    Expires = DateTimeOffset.UtcNow.Add(_cookieExpiry)
                });
            }
            
            return Ok(res);
        }

        [HttpPost("refresh")]
        public async Task<ActionResult<ResponseDto<TokenDto>>> Refresh()
        {
            if (!Request.Cookies.TryGetValue("refresh_token", out var oldRaw)) return Unauthorized();
            // HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown"
            var res = await auth.RefreshAsync(oldRaw,Request.HttpContext.Connection.RemoteIpAddress is null ? "" : Request.HttpContext.Connection.RemoteIpAddress.ToString());
             // set HttpOnly cookies (path limited to auth endpoint)
             if(res.data is not null)
            {
                Response.Cookies.Append("refresh_token", res.data.RefreshToken, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Path = "/api/Auth",
                    Expires = DateTimeOffset.UtcNow.Add(_cookieExpiry)
                });
            }
            
            return Ok(res);

        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<ActionResult<ResponseDto<bool>>> Revoke()
        {
            if (Request.Cookies.TryGetValue("refresh_token", out var raw))
            {
                var res = await auth.RevokeAsync(raw);
                Response.Cookies.Delete("refresh_token", new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Path = "/api/Auth",
                });
                return Ok(res);
            }

            return Unauthorized();
        }

        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> Me()
        {

            var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";
            var rol_id = User.FindFirst("role_id")?.Value ?? "";

            Console.WriteLine($"User: {User.FindFirst("sub")?.Value}, Role: {User.FindFirst("role_id")?.Value}");

            var locations = username.Equals("") ? new List<int>() : await oper.GetOperatorLocationsAsync(username);
            var permissions = rol_id.Equals("") ? new List<PermissionDto>() : await oper.GetOperatorFeaturesAsync(int.Parse(rol_id));

            return Ok(
                new
                {
                    Auth = true,
                    Locations = locations,
                    Role = int.Parse(rol_id),
                    Permissions = permissions
                }
                );
        }


    }
}

