

using System.Text.Json;
using Aero.Application.DTOs;
using Aero.Application.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aero.Api.Controllers.V1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AuthController(IAuthService service) : ControllerBase
    {
        private readonly TimeSpan _cookieExpiry = TimeSpan.FromHours(3);

        [HttpPost("login")]
        public async Task<ActionResult<ResponseDto<TokenDto>>> Login([FromBody] LoginDto model)
        {
            var res = await service.LoginAsync(model, Request.HttpContext.Connection.RemoteIpAddress is null ? "" : Request.HttpContext.Connection.RemoteIpAddress.ToString());
            // set HttpOnly cookies (path limited to auth endpoint)
            if(res.data is not null)
            {
                Response.Cookies.Append("refresh_token", res.data.RefreshToken, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Path = "/api/v1/Auth",
                    Expires = DateTimeOffset.UtcNow.Add(_cookieExpiry)
                });
            }
            
            return Ok(res);
        }

        [HttpPost("refresh")]
        [Authorize]
        public async Task<ActionResult<ResponseDto<TokenDto>>> Refresh()
        {
            if (!Request.Cookies.TryGetValue("refresh_token", out var oldRaw)) return Unauthorized();
            // HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown"
            var res = await service.RefreshAsync(oldRaw,Request.HttpContext.Connection.RemoteIpAddress is null ? "" : Request.HttpContext.Connection.RemoteIpAddress.ToString());
             // set HttpOnly cookies (path limited to auth endpoint)
             if(res.data is not null)
            {
                Response.Cookies.Append("refresh_token", res.data.RefreshToken, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Path = "/api/v1/Auth",
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
                var res = await service.RevokeAsync(raw);
                Response.Cookies.Delete("refresh_token", new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Path = "/api/v1/Auth",
                });
                return Ok(res);
            }

            return Unauthorized();
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
            } : JsonSerializer.Deserialize<Aero.Application.DTOs.Role>(roleJson);

            var info = new TokenInfo
            {
                User = user,
                Locations = loc,
                Role = rol
            };
            var dto = new TokenDetail
            {
                Auth = true,
                //info = info,
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

