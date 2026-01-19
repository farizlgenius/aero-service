
using AeroService.DTO;
using AeroService.DTO.MonitorPoint;
using AeroService.Entity;
using AeroService.Service;
using AeroService.Service.Impl;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;

namespace AeroService.Controllers.V1
{
    [Route("api/v1/[controller]")]
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

        [HttpGet("/api/v1/{location}/[controller]")]
        [Authorize]
        public async Task<ActionResult<ResponseDto<IEnumerable<MonitorPointDto>>>> GetByLocationAsync(short location)
        {
            var res = await service.GetByLocationAsync(location);
            return Ok(res);
        }

        [HttpGet("{mac}/{component}")]
        [Authorize]
        public async Task<ActionResult<ResponseDto<MonitorPointDto>>> GetByComponentAsync(string mac, short component)
        {
            var res = await service.GetByIdAndMacAsync(mac,component);
            return Ok(res);
        }

        [HttpGet("{mac}")]
        [Authorize]
        public async Task<ActionResult<ResponseDto<IEnumerable<MonitorPointDto>>>> GetByMacAsync(string mac)
        {
            var res = await service.GetByIdAndMacAsync(mac);
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
        public async Task<ActionResult<ResponseDto<IEnumerable<ResponseDto<bool>>>>> DeleteRangeAsync([FromBody] List<short> components)
        {
            var res = await service.DeleteRangeAsync(components);
            return Ok(res);
        }



        [HttpGet("ip/{mac}/{component}")]
        [Authorize]
        public async Task<ActionResult<ResponseDto<IEnumerable<short>>>> GetAvailableIp(string mac ,short component)
        {
            var res = await service.GetAvailableIp(mac, component);
            return Ok(res);
        }

        [HttpGet("status/{mac}/{component}")]
        [Authorize]
        public async Task<ActionResult<ResponseDto<bool>>> GetStatusAsync(string mac,short component)
        {
            var res = await service.GetStatusAsync(mac,component);
            return Ok(res);
        }

        [HttpGet("lf")]
        [Authorize]
        public async Task<ActionResult<ResponseDto<IEnumerable<ModeDto>>>> GetLogFunctionAsync()
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
