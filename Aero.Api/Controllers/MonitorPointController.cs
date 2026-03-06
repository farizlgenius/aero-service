using Aero.Application.DTOs;
using Aero.Application.Interface;
using Aero.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aero.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MonitorPointController(IMonitorPointService service) : ControllerBase
    {

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<ResponseDto<IEnumerable<MonitorPointDto>>>> GetAsync()
        {
            var res = await service.GetAsync();
            return Ok(res);
        }

        [HttpGet("/api/{location}/[controller]")]
        [Authorize]
        public async Task<ActionResult<ResponseDto<IEnumerable<MonitorPointDto>>>> GetByLocationAsync(short location)
        {
            var res = await service.GetByLocationAsync(location);
            return Ok(res);
        }

        [HttpGet("/api/{location}/[controller]/pagination")]
        [Authorize]
        public async Task<ActionResult<ResponseDto<Pagination<MonitorPointDto>>>> GetPaginationAsync([FromQuery]PaginationParamsWithFilter param,short location)
        {
            var res = await service.GetPaginationAsync(param,location);
            return Ok(res);
        }

        [HttpGet("component/{component}")]
        [Authorize]
        public async Task<ActionResult<ResponseDto<MonitorPointDto>>> GetByComponentAsync(short component)
        {
            var res = await service.GetByIdAsync(component);
            return Ok(res);
        }

        [HttpGet("device/{device}")]
        [Authorize]
        public async Task<ActionResult<ResponseDto<IEnumerable<MonitorPointDto>>>> GetByDviceIdAsync(int device)
        {
            var res = await service.GetByDviceIdAsync(device);
            return Ok(res);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<ResponseDto<MonitorPointDto>>> CreateAsync([FromBody] MonitorPointDto dto)
        {
           var res = await service.CreateAsync(dto);
            return Ok(res);
        }

        [HttpPut]
        [Authorize]
        public async Task<ActionResult<ResponseDto<MonitorPointDto>>> UpdateAsync([FromBody] MonitorPointDto dto)
        {
            var res = await service.UpdateAsync(dto);
            return Ok(res);
        }

        [HttpDelete("{component}")]
        [Authorize]
        public async Task<ActionResult<ResponseDto<MonitorPointDto>>> DeleteAsync(short component)
        {
            var res = await service.DeleteAsync(component);
            return Ok(res);
        }

        [HttpPost("delete/range")]
        [Authorize]
        public async Task<ActionResult<ResponseDto<IEnumerable<ResponseDto<bool>>>>> DeleteRangeAsync([FromBody] List<int> ids)
        {
            var res = await service.DeleteRangeAsync(ids);
            return Ok(res);
        }



        [HttpGet("ip/{id}")]
        [Authorize]
        public async Task<ActionResult<ResponseDto<IEnumerable<short>>>> GetAvailableIp(int id)
        {
            var res = await service.GetAvailableIp(id);
            return Ok(res);
        }

        [HttpGet("status/{component}")]
        [Authorize]
        public async Task<ActionResult<ResponseDto<bool>>> GetStatusAsync(short component)
        {
            var res = await service.GetStatusByIdAsync(component);
            return Ok(res);
        }

        [HttpGet("lf")]
        [Authorize]
        public async Task<ActionResult<ResponseDto<IEnumerable<Mode>>>> GetLogFunctionAsync()
        {
            var res = await service.GetLogFunctionAsync();
            return Ok(res);
        }


        [HttpGet("input/mode")]
        [Authorize]
        public async Task<ActionResult<ResponseDto<IEnumerable<IpModeDto>>>> GetInputMode()
        {
            var res = await service.GetModeAsync(0);
            return Ok(res);
        }

        [HttpGet("mode")]
        [Authorize]
        public async Task<ActionResult<ResponseDto<IEnumerable<IpModeDto>>>> GetMonitorPointMode()
        {
            var res = await service.GetModeAsync(1);
            return Ok(res);
        }

        [HttpPost("mask")]
        [Authorize]
        public async Task<ActionResult<ResponseDto<MonitorPointDto>>> Mask(MonitorPointDto dto)
        {
            var res = await service.MaskAsync(dto, true);
            return Ok(res);
        }

        [HttpPost("unmask")]
        [Authorize]
        public async Task<ActionResult<ResponseDto<MonitorPointDto>>> UnMask(MonitorPointDto dto)
        {
            var res = await service.MaskAsync(dto,false);
            return Ok(res);
        }
    }
}
