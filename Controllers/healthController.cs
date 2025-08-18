using HIDAeroService.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HIDAeroService.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public sealed class healthController : ControllerBase
    {
        [HttpGet]
        public ActionResult<GenericDto<HealthDto>> GetHealth() 
        {
            GenericDto<HealthDto> dto = new GenericDto<HealthDto>()
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
