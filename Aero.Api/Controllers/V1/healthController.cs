using AeroService.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Aero.Api.Controllers.V1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public sealed class HealthController : ControllerBase
    {
        [HttpGet]
        public ActionResult<ResponseDto<HealthDto>> GetHealth()
        {
            ResponseDto<HealthDto> dto = new ResponseDto<HealthDto>()
            {
                code = HttpStatusCode.OK,
                message = "Success",
                timestamp = DateTime.UtcNow,
                data = new HealthDto { ServerStatus = "UP" }
            };
            return Ok(dto);
        }
    }
}
