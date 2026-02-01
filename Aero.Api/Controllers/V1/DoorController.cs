using Aero.Application.DTOs;
using Aero.Application.Interface;
using Aero.Domain.Entities;
using Aero.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Aero.Api.Controllers.V1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class DoorController(IDoorService doorService) : ControllerBase
    {


        [HttpGet]
        public async Task<ActionResult<ResponseDto<IEnumerable<DoorDto>>>> GetAsync()
        {
            var res = await doorService.GetAsync();
            return Ok(res);
        }

        [HttpGet("/api/v1/{location}/[controller]")]
        public async Task<ActionResult<ResponseDto<IEnumerable<DoorDto>>>> GetByLocationIdAsync(short location)
        {
            var res = await doorService.GetByLocationIdAsync(location);
            return Ok(res);
        }

        [HttpGet("mac/{mac}")]
        public async Task<ActionResult<ResponseDto<DoorDto>>> GetByMacAsync(string mac)
        {
            var res = await doorService.GetByMacAsync(mac);
            return Ok(res);

        }

        [HttpGet("component/{component}")]
        public async Task<ActionResult<ResponseDto<DoorDto>>> GetByComponentAsync(short component)
        {
            var res = await doorService.GetByComponentAsync(component);
            return Ok(res);

        }

        [HttpPost]
        public async Task<ActionResult<ResponseDto<DoorDto>>> CreateAsync([FromBody]DoorDto dto)
        {
            var res = await doorService.CreateAsync(dto);
            return Ok(res);
        }

        [HttpDelete("{component}")]
        public async Task<ActionResult<ResponseDto<DoorDto>>> DeleteAsync(short component)
        {
            var res = await doorService.DeleteAsync(component);
            return Ok(res);
        }

        [HttpPut]
        public async Task<ActionResult<ResponseDto<DoorDto>>> UpdateAsync([FromBody]DoorDto dto)
        {
            var res = await doorService.UpdateAsync(dto);
            return Ok(res);
        }


        [HttpPost("unlock/{mac}/{component}")]
        public async Task<ActionResult<ResponseDto<bool>>> UnlockAsync(string mac,short component)
        {
            var res = await doorService.UnlockAsync(mac,component);
            return Ok(res);
        }

        [HttpGet("reader/mode")]
        public async Task<ActionResult<ResponseDto<Mode>>> ReaderModeAsync()
        {
            var res = await doorService.GetModeAsync((int)DoorServiceMode.ReaderMode);
            return Ok(res);
        }

        [HttpGet("strike/mode")]
        public async Task<ActionResult<ResponseDto<Mode>>> StrikeModeAsync()
        {
            var res = await doorService.GetModeAsync((int)DoorServiceMode.StrikeMode);
            return Ok(res);
        }

        [HttpGet("spareflag")]
        public async Task<ActionResult<ResponseDto<Mode>>> SpareFlagAsync()
        {
            var res = await doorService.GetModeAsync((int)DoorServiceMode.SpareFlag);
            return Ok(res);
        }


        [HttpGet("accesscontrolflag")]
        public async Task<ActionResult<ResponseDto<Mode>>> AccessControlFlagAsync()
        {
            var res = await doorService.GetModeAsync((int)DoorServiceMode.AccessControlFlag);
            return Ok(res);
        }



        [HttpGet("mode")]
        public async Task<ActionResult<ResponseDto<Mode>>> AcrModeAsync()
        {
            var res = await doorService.GetModeAsync((int)DoorServiceMode.AcrMode);
            return Ok(res);
        }

        [HttpGet("apb/mode")]
        public async Task<ActionResult<ResponseDto<Mode>>> ApbModeAsync()
        {
            var res = await doorService.GetModeAsync((int)DoorServiceMode.ApbMode);
            return Ok(res);
        }

        [HttpGet("readerout/mode")]
        public async Task<ActionResult<ResponseDto<Mode>>> ReaderOutConfigurationAsync()
        {
            var res = await doorService.GetModeAsync((int)DoorServiceMode.ReaderOut);
            return Ok(res);
        }

        [HttpGet("reader/{mac}/{component}")]
        public async Task<ActionResult<ResponseDto<short>>> AvailableReaderAsync(string mac,short component)
        {
            var res = await doorService.AvailableReaderAsync(mac,component);
            return Ok(res);
        }

        [HttpGet("status/{mac}/{component}")]
        public async Task<ActionResult<ResponseDto<bool>>> GetStatusAsync(string mac, short component)
        {
            var res = await doorService.GetStatusAsync(mac,component);
            return Ok(res);
        }



        [HttpPost("mode")]
        public async Task<ActionResult<ResponseDto<bool>>> ChangeModeAsync([FromBody] ChangeDoorModeDto dto)
        {
            var res = await doorService.ChangeModeAsync(dto);
            return Ok(res);
        }

        [HttpGet("osdp/baudrate")]
        public async Task<ActionResult<ResponseDto<Mode>>> GetOsdpBaudRate()
        {
            var res = await doorService.GetOsdpBaudRate();
            return Ok(res);
        }

        [HttpGet("osdp/address")]
        public async Task<ActionResult<ResponseDto<Mode>>> GetOsdpAddress()
        {
            var res = await doorService.GetOsdpAddress();
            return Ok(res); 
        }

        [HttpGet("osdp/address/{mac}/{component}")]
        public async Task<ActionResult<ResponseDto<Mode>>> GetAvailableOsdpAddress(string mac,short component)
        {
            var res = await doorService.GetAvailableOsdpAddress(mac,component);
            return Ok(res);
        }


    }
}
