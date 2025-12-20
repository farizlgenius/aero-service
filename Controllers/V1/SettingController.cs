using HIDAeroService.DTO.Operator;
using HIDAeroService.DTO;
using HIDAeroService.Service.Impl;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HIDAeroService.Service;
using Microsoft.AspNetCore.Authorization;

namespace HIDAeroService.Controllers.V1
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
