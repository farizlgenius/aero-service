using Aero.Application.DTOs;
using Aero.Application.Interface;
using Aero.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aero.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ControlPointController(IControlPointService service) : ControllerBase
    {

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<ResponseDto<IEnumerable<ControlPointDto>>>> GetAsync()
        {
            var res = await service.GetAsync();
            return Ok(res);
        }

        [HttpGet("/api/v1/{location}/[controller]")]
        [Authorize]
        public async Task<ActionResult<ResponseDto<IEnumerable<ControlPointDto>>>> GetByLocationAsync(short location)
        {
            var res = await service.GetByLocationAsync(location);
            return Ok(res);
        }

        [HttpGet("/api/v1/{location}/[controller]/pagination")]
        [Authorize]
        public async Task<ActionResult<ResponseDto<Pagination<ControlPointDto>>>> GetPaginationAsync( [FromQuery] PaginationParamsWithFilter param,short location)
        {
            var res = await service.GetPaginationAsync(param,location);
            return Ok(res);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ResponseDto<ControlPointDto>>> GetByIdAsync(int id)
        {
            var res = await service.GetByIdAsync(id);
            return StatusCode((int)res.code, res);
        }

        [HttpPost("control")]
        [Authorize]
        public async Task<ActionResult<ResponseDto<ControlPointDto>>> CreateControlPointAsync([FromBody] CreateControlPointDto dto)
        {
            var res = await service.CreateAsync(dto);
            return Ok(res);
        }

        [HttpPut]
        [Authorize]
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
        [Authorize]
        public async Task<ActionResult<ResponseDto<bool>>> ControlPointTriggerAsync(ToggleControlPointDto dto)
        {
            var res = await service.ToggleAsync(dto);
            return Ok(res);
        }



        [HttpGet("mode/offline")]
        [Authorize]
        public async Task<ActionResult<ResponseDto<List<Mode>>>> GetOfflineModeAsync()
        {
            var res = await service.GetModeAsync(0);
            return StatusCode((int)res.code, res);
        }

        [HttpGet("mode/relay")]
        [Authorize]
        public async Task<ActionResult<ResponseDto<List<Mode>>>> GetRelayModeAsync()
        {
            var res = await service.GetModeAsync(1);
            return StatusCode((int)res.code, res);
        }

        [HttpGet("op/{id}/{device}")]
        [Authorize]
        public async Task<ActionResult<ResponseDto<List<short>>>> GetAvailableOpAsync(int id,int device)
        {
            var res = await service.GetAvailableOpAsync(device,id);
            return Ok(res);
        }

        [HttpGet("status/{device}/{driver}")]
        public async Task<ActionResult<ResponseDto<bool>>> GetStatusAsync(int device,short driver)
        {
            var res = await service.GetStatusAsync(device,driver);
            return Ok(res);

        }

        [HttpPost("delete/range")]
        [Authorize]
        public async Task<ActionResult<ResponseDto<IEnumerable<ResponseDto<bool>>>>> DeleteRangeAsync([FromBody] List<int> ids) 
        {
            var res = await service.DeleteRangeAsync(ids);
            return Ok(res);
        }

    }
}
