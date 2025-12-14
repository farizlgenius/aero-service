
using HIDAeroService.DTO;
using HIDAeroService.DTO.TimeZone;
using HIDAeroService.Entity;
using HIDAeroService.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.Net;

namespace HIDAeroService.Controllers.V1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public sealed class TimeZoneController(ITimeZoneService baseService) : ControllerBase 
    { 
        [HttpGet]
        public async Task<ActionResult<ResponseDto<IEnumerable<TimeZoneDto>>>> GetAsync()
        {
            var res = await baseService.GetAsync();
            return Ok(res);
        }

        [HttpPost]
        public async Task<ActionResult<ResponseDto<TimeZoneDto>>> CreateAsync([FromBody] CreateTimeZoneDto dto) 
        {
            var res = await baseService.CreateAsync(dto);
            return Ok(res);
        }

        [HttpPut]
        public async Task<ActionResult<ResponseDto<TimeZoneDto>>> UpdateAsync([FromBody] TimeZoneDto dto)
        {
            var res = await baseService.UpdateAsync(dto);
            return Ok(res);
        }

        [HttpDelete("{component}")]
        public async Task<ActionResult<ResponseDto<TimeZoneDto>>> DeleteAsync(short component)
        {
            var res = await baseService.DeleteAsync(component);
            return Ok(res);
        }

        [HttpGet("{component}")]
        public async Task<ActionResult<ResponseDto<TimeZoneDto>>> GetByComponentAsync(short component)
        {
            var res = await baseService.GetByComponentIdAsync(component);
            return Ok(res);
        }

        [HttpGet("mode")]
        public async Task<ActionResult<ResponseDto<ModeDto>>> GetModeAsync()
        {
            var res = await baseService.GetModeAsync(0);
            return Ok(res);
        }

        [HttpGet("command")]
        public async Task<ActionResult<ResponseDto<IEnumerable<ModeDto>>>> GetCommandAsync()
        {
            var res = await baseService.GetCommandAsync();
            return Ok(res);
        }

    }
}
