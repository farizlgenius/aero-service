
using Microsoft.AspNetCore.Mvc;
using AeroService.Service;
using Microsoft.AspNetCore.Authorization;
using Aero.Application.Interface;
using Aero.Application.DTOs;

namespace Aero.Api.Controllers
{
    [Route("api/[controller]")]
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

        [HttpGet("led")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<LedDto>>> GetLedSettingAsync()
        {
            var res = await service.GetLedSettingAsync();
            return Ok(res);
        }

        [HttpGet("led/{id}")]
        [Authorize]
        public async Task<ActionResult<LedDto>> GetLedSettingByIdAsync(int id)
        {
            var res = await service.GetLedSettingByIdAsync(id);
            return Ok(res);
        }

        [HttpPut("led")]
        public async Task<ActionResult<ResponseDto<LedDto>>> UpdateLedSettingAsync([FromBody] LedDto dto )
        {
            var res = await service.UpdateLedSettingAsync(dto);
            return Ok(res);
        }
    }
}
