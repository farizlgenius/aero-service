
using AeroService.DTO;
using AeroService.DTO.IdReport;
using AeroService.Service.Impl;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Aero.Api.Controllers.V1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class IdReportController(IdReportService idReportService) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<ResponseDto<IEnumerable<IdReportDto>>>> GetAsync()
        {
            var res = await idReportService.GetAsync();
            return Ok(res);
        }

        [HttpGet("count")]
        public async Task<ActionResult<ResponseDto<int>>> GetCountAsync()
        {
            var res = await idReportService.GetCount();
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
