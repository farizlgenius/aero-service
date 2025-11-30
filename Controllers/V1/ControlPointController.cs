using HIDAeroService.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HID.Aero.ScpdNet.Wrapper;
using HIDAeroService.Entity;
using HIDAeroService.Service;
using HIDAeroService.DTO;
using HIDAeroService.DTO.Output;
using HIDAeroService.DTO.ControlPoint;

namespace HIDAeroService.Controllers.V1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ControlPointController(IControlPointService cpService) : ControllerBase
    {


        [HttpGet]
        public async Task<ActionResult<ResponseDto<IEnumerable<ControlPointDto>>>> GetAsync()
        {
            var res = await cpService.GetAsync();
            return Ok(res);
        }

        [HttpGet("/api/v1/{location}/[controller]")]
        public async Task<ActionResult<ResponseDto<IEnumerable<ControlPointDto>>>> GetByLocationAsync(short location)
        {
            var res = await cpService.GetByLocationAsync(location);
            return Ok(res);
        }

        [HttpGet("{mac}/{id}")]
        public async Task<ActionResult<ResponseDto<ControlPointDto>>> GetByComponentAsync(string mac,short id)
        {
            var res = await cpService.GetByMacAndIdAsync(mac,id);
            return StatusCode((int)res.code, res);
        }

        [HttpPost("control")]
        public async Task<ActionResult<ResponseDto<ControlPointDto>>> CreateControlPointAsync([FromBody] ControlPointDto dto)
        {
            var res = await cpService.CreateOutputAsync(dto);
            return Ok(res);
        }

        [HttpPut]
        public async Task<ActionResult<ResponseDto<ControlPointDto>>> UpdateAsync([FromBody] ControlPointDto dto)
        {
            var res = await cpService.UpdateAsync(dto);
            return Ok(res);
        }

        [HttpDelete("{mac}/{component}")]
        public async Task<ActionResult<ResponseDto<ControlPointDto>>> DeleteAsync(string mac, short component)
        {
            var res = await cpService.DeleteAsync(mac, component);
            return Ok(res);
        }


        [HttpPost("trigger")]
        public async Task<ActionResult<ResponseDto<bool>>> ControlPointTriggerAsync(ToggleControlPointDto dto)
        {
            var res = await cpService.ToggleAsync(dto);
            return Ok(res);
        }



        [HttpGet("mode/offline")]
        public async Task<ActionResult<ResponseDto<List<ModeDto>>>> GetOfflineModeAsync()
        {
            var res = await cpService.GetModeAsync(0);
            return StatusCode((int)res.code, res);
        }

        [HttpGet("mode/relay")]
        public async Task<ActionResult<ResponseDto<List<ModeDto>>>> GetRelayModeAsync()
        {
            var res = await cpService.GetModeAsync(1);
            return StatusCode((int)res.code, res);
        }

        [HttpGet("op/{mac}/{component}")]
        public async Task<ActionResult<ResponseDto<List<short>>>> GetAvailableOpAsync(string mac,short component)
        {
            var res = await cpService.GetAvailableOpAsync(mac,component);
            return Ok(res);
        }

        [HttpGet("status/{mac}/{component}")]
        public async Task<ActionResult<ResponseDto<bool>>> GetStatusAsync(string mac,short component)
        {
            var res = await cpService.GetStatusAsync(mac,component);
            return Ok(res);

        }

    }
}
