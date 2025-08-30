using HIDAeroService.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HIDAeroService.Controllers.V1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public sealed class healthController : ControllerBase
    {
        [HttpGet]
        public ActionResult<BaseDto<HealthDto>> GetHealth()
        {
            BaseDto<HealthDto> dto = new BaseDto<HealthDto>()
            {
                StatusCode = 200,
                StatusDesc = "Success",
                Time = DateTime.Now.ToLocalTime(),
                Content = new HealthDto { ServerStatus = "UP" }
            };
            return Ok(dto);
        }
    }
}
