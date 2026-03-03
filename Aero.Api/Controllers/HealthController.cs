using Aero.Application.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Aero.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public sealed class HealthController : ControllerBase
    {
        [HttpGet]
        public ActionResult<ResponseDto<HealthDto>> GetHealth()
        {
            ResponseDto<HealthDto> dto = new ResponseDto<HealthDto>(DateTime.UtcNow,HttpStatusCode.OK,new HealthDto("Up"),"Online",[]);
            return Ok(dto);
        }
    }
}
