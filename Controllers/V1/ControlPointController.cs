using AeroService.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HID.Aero.ScpdNet.Wrapper;
using AeroService.Entity;
using AeroService.Service;
using AeroService.DTO;
using AeroService.DTO.Output;
using AeroService.DTO.ControlPoint;

namespace AeroService.Controllers.V1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ControlPointController(IControlPointService service) : ControllerBase
    {


        [HttpGet]
        public async Task<ActionResult<ResponseDto<IEnumerable<ControlPointDto>>>> GetAsync()
        {
            var res = await service.GetAsync();
            return Ok(res);
        }

        [HttpGet("/api/v1/{location}/[controller]")]
        public async Task<ActionResult<ResponseDto<IEnumerable<ControlPointDto>>>> GetByLocationAsync(short location)
        {
            var res = await service.GetByLocationAsync(location);
            return Ok(res);
        }

        [HttpGet("{mac}/{id}")]
        public async Task<ActionResult<ResponseDto<ControlPointDto>>> GetByComponentAsync(string mac,short id)
        {
            var res = await service.GetByMacAndIdAsync(mac,id);
            return StatusCode((int)res.code, res);
        }

        [HttpPost("control")]
        public async Task<ActionResult<ResponseDto<ControlPointDto>>> CreateControlPointAsync([FromBody] ControlPointDto dto)
        {
            var res = await service.CreateAsync(dto);
            return Ok(res);
        }

        [HttpPut]
        public async Task<ActionResult<ResponseDto<ControlPointDto>>> UpdateAsync([FromBody] ControlPointDto dto)
        {
            var res = await service.UpdateAsync(dto);
            return Ok(res);
        }

        [HttpDelete("{component}")]
        public async Task<ActionResult<ResponseDto<ControlPointDto>>> DeleteAsync(short component)
        {
            var res = await service.DeleteAsync(component);
            return Ok(res);
        }


        [HttpPost("trigger")]
        public async Task<ActionResult<ResponseDto<bool>>> ControlPointTriggerAsync(ToggleControlPointDto dto)
        {
            var res = await service.ToggleAsync(dto);
            return Ok(res);
        }



        [HttpGet("mode/offline")]
        public async Task<ActionResult<ResponseDto<List<ModeDto>>>> GetOfflineModeAsync()
        {
            var res = await service.GetModeAsync(0);
            return StatusCode((int)res.code, res);
        }

        [HttpGet("mode/relay")]
        public async Task<ActionResult<ResponseDto<List<ModeDto>>>> GetRelayModeAsync()
        {
            var res = await service.GetModeAsync(1);
            return StatusCode((int)res.code, res);
        }

        [HttpGet("op/{mac}/{component}")]
        public async Task<ActionResult<ResponseDto<List<short>>>> GetAvailableOpAsync(string mac,short component)
        {
            var res = await service.GetAvailableOpAsync(mac,component);
            return Ok(res);
        }

        [HttpGet("status/{mac}/{component}")]
        public async Task<ActionResult<ResponseDto<bool>>> GetStatusAsync(string mac,short component)
        {
            var res = await service.GetStatusAsync(mac,component);
            return Ok(res);

        }

        [HttpPost("delete/range")]
        public async Task<ActionResult<ResponseDto<IEnumerable<ResponseDto<bool>>>>> DeleteRangeAsync([FromBody] List<short> components) 
        {
            var res = await service.DeleteRangeAsync(components);
            return Ok(res);
        }

    }
}
