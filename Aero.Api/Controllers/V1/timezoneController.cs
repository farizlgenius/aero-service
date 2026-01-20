
using AeroService.DTO;
using AeroService.DTO.TimeZone;
using AeroService.Entity;
using AeroService.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.Net;

namespace Aero.Api.Controllers.V1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public sealed class TimeZoneController(ITimeZoneService service) : ControllerBase 
    { 
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<ResponseDto<IEnumerable<TimeZoneDto>>>> GetAsync()
        {
            var res = await service.GetAsync();
            return Ok(res);
        }

        [HttpGet("/api/v1/{location}/[controller]")]
        [Authorize]
        public async Task<ActionResult<ResponseDto<IEnumerable<TimeZoneDto>>>> GetByLocationAsync(short location) 
        {
            var res = await service.GetByLocationAsync(location);
            return Ok(res);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<ResponseDto<TimeZoneDto>>> CreateAsync([FromBody] CreateTimeZoneDto dto) 
        {
            var res = await service.CreateAsync(dto);
            return Ok(res);
        }

        [HttpPut]
        [Authorize]
        public async Task<ActionResult<ResponseDto<TimeZoneDto>>> UpdateAsync([FromBody] TimeZoneDto dto)
        {
            var res = await service.UpdateAsync(dto);
            return Ok(res);
        }

        [HttpDelete("{component}")]
        [Authorize]
        public async Task<ActionResult<ResponseDto<TimeZoneDto>>> DeleteAsync(short component)
        {
            var res = await service.DeleteAsync(component);
            return Ok(res);
        }

        [HttpGet("{component}")]
        [Authorize]
        public async Task<ActionResult<ResponseDto<TimeZoneDto>>> GetByComponentAsync(short component)
        {
            var res = await service.GetByComponentIdAsync(component);
            return Ok(res);
        }

        [HttpGet("mode")]
        [Authorize]
        public async Task<ActionResult<ResponseDto<ModeDto>>> GetModeAsync()
        {
            var res = await service.GetModeAsync(0);
            return Ok(res);
        }

        [HttpGet("command")]
        [Authorize]
        public async Task<ActionResult<ResponseDto<IEnumerable<ModeDto>>>> GetCommandAsync()
        {
            var res = await service.GetCommandAsync();
            return Ok(res);
        }

    }
}
