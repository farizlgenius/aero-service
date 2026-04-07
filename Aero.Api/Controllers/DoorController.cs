using Aero.Application.DTOs;
using Aero.Application.Interface;
using Aero.Domain.Entities;
using Aero.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aero.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoorController(IDoorService doorService) : ControllerBase
    {


        [HttpGet]
        [Authorize]
        public async Task<ActionResult<ResponseDto<IEnumerable<DoorDto>>>> GetAsync()
        {
            var res = await doorService.GetAsync();
            return Ok(res);
        }

        [HttpGet("/api/{location}/[controller]")]
        [Authorize]
        public async Task<ActionResult<ResponseDto<IEnumerable<DoorDto>>>> GetByLocationIdAsync(short location)
        {
            var res = await doorService.GetByLocationIdAsync(location);
            return Ok(res);
        }

        [HttpGet("/api/{location}/[controller]/pagination")]
        [Authorize]
        public async Task<ActionResult<ResponseDto<IEnumerable<DoorDto>>>> GetPaginationAsync([FromQuery] PaginationParamsWithFilter param,short location)
        {
            var res = await doorService.GetPaginationAsync(param,location);
            return Ok(res);
        }

        [HttpGet("{deviceId}")]
        [Authorize]
        public async Task<ActionResult<ResponseDto<DoorDto>>> GetByIdAsync(int deviceId)
        {
            var res = await doorService.GetByDeviceIdAsync(deviceId);
            return Ok(res);

        }

        [HttpGet("component/{component}")]
        [Authorize]
        public async Task<ActionResult<ResponseDto<DoorDto>>> GetByComponentAsync(short component)
        {
            var res = await doorService.GetByComponentAsync(component);
            return Ok(res);

        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<ResponseDto<DoorDto>>> CreateAsync([FromBody]CreateDoorDto dto)
        {
            var res = await doorService.CreateAsync(dto);
            return Ok(res);
        }

        [HttpDelete("{component}")]
        [Authorize]
        public async Task<ActionResult<ResponseDto<DoorDto>>> DeleteAsync(short component)
        {
            var res = await doorService.DeleteAsync(component);
            return Ok(res);
        }

        [HttpPut]
        [Authorize]
        public async Task<ActionResult<ResponseDto<DoorDto>>> UpdateAsync([FromBody]DoorDto dto)
        {
            var res = await doorService.UpdateAsync(dto);
            return Ok(res);
        }


        [HttpPost("unlock/{id}")]
        [Authorize]
        public async Task<ActionResult<ResponseDto<bool>>> UnlockAsync(int id)
        {
            var res = await doorService.UnlockByIdAsync(id);
            return Ok(res);
        }

        [HttpGet("reader/mode")]
        [Authorize]
        public async Task<ActionResult<ResponseDto<ModeDto>>> ReaderModeAsync()
        {
            var res = await doorService.GetModeAsync((int)DoorServiceMode.ReaderMode);
            return Ok(res);
        }

        [HttpGet("strike/mode")]
        [Authorize]
        public async Task<ActionResult<ResponseDto<ModeDto>>> StrikeModeAsync()
        {
            var res = await doorService.GetModeAsync((int)DoorServiceMode.StrikeMode);
            return Ok(res);
        }

        [HttpGet("spareflag")]
        [Authorize]
        public async Task<ActionResult<ResponseDto<ModeDto>>> SpareFlagAsync()
        {
            var res = await doorService.GetModeAsync((int)DoorServiceMode.SpareFlag);
            return Ok(res);
        }


        [HttpGet("accesscontrolflag")]
        [Authorize]
        public async Task<ActionResult<ResponseDto<ModeDto>>> AccessControlFlagAsync()
        {
            var res = await doorService.GetModeAsync((int)DoorServiceMode.AccessControlFlag);
            return Ok(res);
        }

        [HttpGet("type")]
        [Authorize]
        public async Task<ActionResult<ResponseDto<ModeDto>>> GetDoorTypeAsync()
        {
            var res = await doorService.GetModeAsync((int)DoorServiceMode.DoorType);
            return Ok(res);
        }

        [HttpGet("mode")]
        [Authorize]
        public async Task<ActionResult<ResponseDto<ModeDto>>> AcrModeAsync()
        {
            var res = await doorService.GetModeAsync((int)DoorServiceMode.AcrMode);
            return Ok(res);
        }

        [HttpGet("apb/mode")]
        [Authorize]
        public async Task<ActionResult<ResponseDto<ModeDto>>> ApbModeAsync()
        {
            var res = await doorService.GetModeAsync((int)DoorServiceMode.ApbMode);
            return Ok(res);
        }

        [HttpGet("readerout/mode")]
        [Authorize]
        public async Task<ActionResult<ResponseDto<ModeDto>>> ReaderOutConfigurationAsync()
        {
            var res = await doorService.GetModeAsync((int)DoorServiceMode.ReaderOut);
            return Ok(res);
        }

        [HttpGet("reader/{moduleId}")]
        [Authorize]
        public async Task<ActionResult<ResponseDto<short>>> AvailableReaderAsync(int moduleId)
        {
            var res = await doorService.AvailableReaderAsync(moduleId);
            return Ok(res);
        }

        [HttpGet("status/{id}")]
        [Authorize]
        public async Task<ActionResult<ResponseDto<bool>>> GetStatusAsync(int id)
        {
            var res = await doorService.GetStatusAsync(id);
            return Ok(res);
        }



        [HttpPost("mode")]
        [Authorize]
        public async Task<ActionResult<ResponseDto<bool>>> ChangeModeAsync([FromBody] ChangeDoorModeDto dto)
        {
            var res = await doorService.ChangeModeAsync(dto);
            return Ok(res);
        }

        [HttpGet("osdp/baudrate")]
        [Authorize]
        public async Task<ActionResult<ResponseDto<ModeDto>>> GetOsdpBaudRate()
        {
            var res = await doorService.GetOsdpBaudRate();
            return Ok(res);
        }

        [HttpGet("osdp/address")]
        [Authorize]
        public async Task<ActionResult<ResponseDto<ModeDto>>> GetOsdpAddress()
        {
            var res = await doorService.GetOsdpAddress();
            return Ok(res); 
        }

        [HttpGet("osdp/address/{module}")]
        [Authorize]
        public async Task<ActionResult<ResponseDto<ModeDto>>> GetAvailableOsdpAddress(int module)
        {
            var res = await doorService.GetAvailableOsdpAddress(module);
            return Ok(res);
        }


    }
}
