
using HIDAeroService.DTO;
using HIDAeroService.DTO.MonitorPoint;
using HIDAeroService.Entity;
using HIDAeroService.Service;
using HIDAeroService.Service.Impl;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;

namespace HIDAeroService.Controllers.V1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class MonitorPointController(IMonitorPointService mpService) : ControllerBase
    {

        [HttpGet]
        public async Task<ActionResult<ResponseDto<IEnumerable<MonitorPointDto>>>> GetAsync()
        {
            var res = await mpService.GetAsync();
            return Ok(res);
        }

        [HttpGet("/api/v1/{location}/[controller]")]
        public async Task<ActionResult<ResponseDto<IEnumerable<MonitorPointDto>>>> GetByLocationAsync(short location)
        {
            var res = await mpService.GetByLocationAsync(location);
            return Ok(res);
        }

        [HttpGet("{mac}/{component}")]
        public async Task<ActionResult<ResponseDto<MonitorPointDto>>> GetByComponentAsync(string mac, short component)
        {
            var res = await mpService.GetByIdAndMacAsync(mac,component);
            return Ok(res);
        }

        [HttpPost]
        public async Task<ActionResult<ResponseDto<MonitorPointDto>>> CreateAsync([FromBody] MonitorPointDto dto)
        {
           var res = await mpService.CreateAsync(dto);
            return Ok(res);
        }

        [HttpPut]
        public async Task<ActionResult<ResponseDto<MonitorPointDto>>> UpdateAsync([FromBody] MonitorPointDto dto)
        {
            var res = await mpService.UpdateAsync(dto);
            return Ok(res);
        }

        [HttpDelete("{mac}/{component}")]
        public async Task<ActionResult<ResponseDto<MonitorPointDto>>> DeleteAsync(string mac, short component)
        {
            var res = await mpService.DeleteAsync(mac,component);
            return Ok(res);
        }



        [HttpGet("ip/{mac}/{component}")]
        public async Task<ActionResult<ResponseDto<IEnumerable<short>>>> GetAvailableIp(string mac ,short component)
        {
            var res = await mpService.GetAvailableIp(mac, component);
            return Ok(res);
        }

        [HttpGet("status/{mac}/{component}")]
        public async Task<ActionResult<ResponseDto<bool>>> GetStatusAsync(string mac,short component)
        {
            var res = await mpService.GetStatusAsync(mac,component);
            return Ok(res);
        }


        [HttpGet("input/mode")]
        public async Task<ActionResult<ResponseDto<IEnumerable<IpModeDto>>>> GetInputMode()
        {
            var res = await mpService.GetModeAsync(0);
            return Ok(res);
        }

        [HttpGet("mode")]
        public async Task<ActionResult<ResponseDto<IEnumerable<IpModeDto>>>> GetMonitorPointMode()
        {
            var res = await mpService.GetModeAsync(1);
            return Ok(res);
        }

        [HttpPost("mask")]
        public async Task<ActionResult<ResponseDto<MonitorPointDto>>> Mask(MonitorPointDto dto)
        {
            var res = await mpService.MaskAsync(dto, true);
            return Ok(res);
        }

        [HttpPost("unmask")]
        public async Task<ActionResult<ResponseDto<MonitorPointDto>>> UnMask(MonitorPointDto dto)
        {
            var res = await mpService.MaskAsync(dto,false);
            return Ok(res);
        }
    }
}
