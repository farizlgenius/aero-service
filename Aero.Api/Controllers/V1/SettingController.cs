
using Microsoft.AspNetCore.Mvc;
using AeroService.Service;
using Microsoft.AspNetCore.Authorization;
using Aero.Application.Interface;
using Aero.Application.DTOs;

namespace Aero.Api.Controllers.V1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class SettingController(ISettingService service ) : ControllerBase
    {
        [HttpGet("password/rule")]
        [Authorize]
        public async Task<ActionResult<ResponseDto<PasswordRuleDto>>> GetPasswordRuleAsync()
        {
            var res = await service.GetPasswordRuleAsync();
            return Ok(res);
        }

        [HttpPost("password/rule")]
        [Authorize]
        public async Task<ActionResult<ResponseDto<bool>>> UpdatePasswordRuleAsync([FromBody] PasswordRuleDto dto)
        {
            var res = await service.UpdatePasswordRuleAsync(dto);
            return Ok(res);
        }
    }
}
