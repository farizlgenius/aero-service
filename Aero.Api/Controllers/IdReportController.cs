using Aero.Application.DTOs;
using Aero.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Aero.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdReportController(IdReportService idReportService) : ControllerBase
    {
        [HttpGet("/api/{location}/[controller]")]
        public async Task<ActionResult<ResponseDto<IEnumerable<IdReportDto>>>> GetAsync(short location)
        {
            var res = await idReportService.GetAsync(location);
            return Ok(res);
        }

        [HttpGet("/api/{location}/[controller]/count")]
        public async Task<ActionResult<ResponseDto<int>>> GetCountAsync(short location)
        {
            var res = await idReportService.GetCount(location);
            return Ok(res);
        }
        [HttpGet("status")]
        public async Task<ActionResult<ResponseDto<bool>>> GetStatusAsync(short id)
        {
            var res = await idReportService.GetStatusAsync(id);
            return Ok(res);
        }



    }
}
