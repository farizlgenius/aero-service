using HIDAeroService.DTO.Auth;
using HIDAeroService.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace HIDAeroService.Controllers.V1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AuthController(IAuthService authService,ITokenService tokenService,IConfiguration configuration) : ControllerBase
    {
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            // TODO: Replace with real user validation (DB, hashed passwords)
            if (!IsValidUser(model.Username, model.Password))
                return Unauthorized();

            var accessToken = tokenService.GenerateAccessToken(model.Username);

            var refreshToken = tokenService.GenerateRefreshToken(model.Username, HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown");
            await tokenService.SaveRefreshTokenAsync(refreshToken);

            return Ok(new
            {
                accessToken,
                refreshToken = refreshToken.Token,
                expiresIn = int.Parse(configuration["Jwt:ExpireMinutes"] ?? "15")
            });
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshRequest req)
        {
            var existing = await tokenService.GetRefreshTokenAsync(req.RefreshToken);
            if (existing == null || !existing.IsActive) return Unauthorized();

            // rotate refresh token (recommended)
            var newRt = await tokenService.RotateRefreshTokenAsync(existing, HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown");
            if (newRt == null) return Unauthorized();

            // issue new access token
            var newAccess = tokenService.GenerateAccessToken(existing.UserName);
            return Ok(new { accessToken = newAccess, refreshToken = newRt.Token });
        }

        [HttpPost("revoke")]
        public async Task<IActionResult> Revoke([FromBody] RefreshRequest req)
        {
            var existing = await tokenService.GetRefreshTokenAsync(req.RefreshToken);
            if (existing == null) return NotFound();
            await tokenService.RevokeRefreshTokenAsync(existing);
            return Ok();
        }

        private bool IsValidUser(string u, string p) => (u == "user" && p == "password"); // demo only

    }
}

