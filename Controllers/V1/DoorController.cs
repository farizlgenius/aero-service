using HIDAeroService.Constants;
using HIDAeroService.DTO;
using HIDAeroService.DTO.Acr;
using HIDAeroService.Entity;
using HIDAeroService.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HIDAeroService.Controllers.V1
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

        [HttpGet("{mac}")]
        public async Task<ActionResult<ResponseDto<DoorDto>>> GetByMacAsync(string mac)
        {
            var res = await doorService.GetByMacAsync(mac);
            return Ok(res);

        }

        [HttpGet("{mac}/{component}")]
        public async Task<ActionResult<ResponseDto<DoorDto>>> GetByComponentAsync(string mac,short component)
        {
            var res = await doorService.GetByComponentAsync(mac, component);
            return Ok(res);

        }

        [HttpPost]
        public async Task<ActionResult<ResponseDto<DoorDto>>> CreateAsync([FromBody]DoorDto dto)
        {
            var res = await doorService.CreateAsync(dto);
            return Ok(res);
        }

        [HttpDelete("{mac}/{component}")]
        public async Task<ActionResult<ResponseDto<DoorDto>>> DeleteAsync(string mac,short component)
        {
            var res = await doorService.DeleteAsync(mac,component);
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
        public async Task<ActionResult<ResponseDto<ModeDto>>> ReaderModeAsync()
        {
            var res = await doorService.GetModeAsync((int)ComponentEnum.AcrServiceMode.ReaderMode);
            return Ok(res);
        }

        [HttpGet("strike/mode")]
        public async Task<ActionResult<ResponseDto<ModeDto>>> StrikeModeAsync()
        {
            var res = await doorService.GetModeAsync((int)ComponentEnum.AcrServiceMode.StrikeMode);
            return Ok(res);
        }

        [HttpGet("spareflag")]
        public async Task<ActionResult<ResponseDto<ModeDto>>> SpareFlagAsync()
        {
            var res = await doorService.GetModeAsync((int)ComponentEnum.AcrServiceMode.SpareFlag);
            return Ok(res);
        }


        [HttpGet("accesscontrolflag")]
        public async Task<ActionResult<ResponseDto<ModeDto>>> AccessControlFlagAsync()
        {
            var res = await doorService.GetModeAsync((int)ComponentEnum.AcrServiceMode.AccessControlFlag);
            return Ok(res);
        }



        [HttpGet("mode")]
        public async Task<ActionResult<ResponseDto<ModeDto>>> AcrModeAsync()
        {
            var res = await doorService.GetModeAsync((int)ComponentEnum.AcrServiceMode.AcrMode);
            return Ok(res);
        }

        [HttpGet("apb/mode")]
        public async Task<ActionResult<ResponseDto<ModeDto>>> ApbModeAsync()
        {
            var res = await doorService.GetModeAsync((int)ComponentEnum.AcrServiceMode.ApbMode);
            return Ok(res);
        }

        [HttpGet("readerout/mode")]
        public async Task<ActionResult<ResponseDto<ModeDto>>> ReaderOutConfigurationAsync()
        {
            var res = await doorService.GetModeAsync((int)ComponentEnum.AcrServiceMode.ReaderOut);
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
        public async Task<ActionResult<ResponseDto<ModeDto>>> GetOsdpBaudRate()
        {
            var res = await doorService.GetOsdpBaudRate();
            return Ok(res);
        }

        [HttpGet("osdp/address")]
        public async Task<ActionResult<ResponseDto<ModeDto>>> GetOsdpAddress()
        {
            var res = await doorService.GetOsdpAddress();
            return Ok(res); 
        }

        [HttpGet("osdp/address/{mac}/{component}")]
        public async Task<ActionResult<ResponseDto<ModeDto>>> GetAvailableOsdpAddress(string mac,short component)
        {
            var res = await doorService.GetAvailableOsdpAddress(mac,component);
            return Ok(res);
        }


    }
}
