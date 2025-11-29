
using Microsoft.AspNetCore.Mvc;
using HIDAeroService.Data;
using HIDAeroService.Entity;
using HIDAeroService.Service;
using HIDAeroService.DTO;
using HIDAeroService.DTO.Scp;
using HIDAeroService.DTO.Hardware;

namespace HIDAeroService.Controllers.V1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class HardwareController(IHardwareService hardwareService) : ControllerBase
    {


        [HttpGet]
        public async Task<ActionResult<ResponseDto<IEnumerable<HardwareDto>>>> GetAsync()
        {
            var res = await hardwareService.GetAsync();
            return Ok(res);
        }

        [HttpGet("/api/v1/{location}/[controller]")]
        public async Task<ActionResult<ResponseDto<IEnumerable<HardwareDto>>>> GetByLocationAsync(short location)
        {
            var res = await hardwareService.GetAsync();
            return Ok(res);
        }

        [HttpGet("{mac}")]
        public async Task<ActionResult<ResponseDto<HardwareDto>>> GetByMacAsync(string mac)
        {
            var res = await hardwareService.GetByMacAsync(mac);
            return StatusCode((int)res.code, res);
        }


        [HttpGet("status/{mac}/{id}")]
        public async Task<ActionResult<ResponseDto<HardwareStatus>>> GetStatusAsync(string mac, short id)
        {
            var res = await hardwareService.GetStatusAsync(mac, id);
            return Ok(res);
        }

        [HttpPost]
        public async Task<ActionResult<ResponseDto<HardwareDto>>> CreateAsync([FromBody] CreateHardwareDto dto)
        {
            var res = await hardwareService.CreateAsync(dto);
            return StatusCode((int)res.code, res);
        }

        [HttpPut]
        public Task<ActionResult<ResponseDto<HardwareDto>>> UpdateAsync([FromBody] HardwareDto dto)
        {
            throw new NotImplementedException();
        }

        [HttpDelete("{mac}")]
        public async Task<ActionResult<ResponseDto<HardwareDto>>> DeleteAsync(string mac)
        {
            var res = await hardwareService.DeleteAsync(mac);
            return StatusCode((int)res.code, res);
        }


        [HttpPost("reset/{mac}")]
        public async Task<ActionResult<ResponseDto<bool>>> ResetByIdAsync(string mac)
        {
            var res = await hardwareService.ResetAsync(mac);
            return Ok(res);
        }

        [HttpPost("reset/id/{id}")]
        public async Task<ActionResult<ResponseDto<bool>>> ResetByIdAsync(short id)
        {
            var res = await hardwareService.ResetAsync(id);
            return Ok(res);
        }



        [HttpPost("upload/{mac}")]
        public async Task<ActionResult<ResponseDto<bool>>> UploadConfigAsync(string mac)
        {
            var res = await hardwareService.UploadConfigAsync(mac);
            return Ok(res);
        }


        [HttpPost("verify/{id}")]
        public async Task<ActionResult<ResponseDto<bool>>> VerifySystemConfigurationByIdAsync(short id)
        {
            var res = await hardwareService.VerifySystemConfigurationAsync(id);
            return Ok(res);
        }

        [HttpDelete("{mac}/{component}")]
        public Task<ActionResult<ResponseDto<HardwareDto>>> DeleteAsync(string mac, short component)
        {
            throw new NotImplementedException();
        }

        [HttpPost("{mac}/{isOn}")]
        public async Task<ActionResult<ResponseDto<bool>>> SetTransactionAsync(string mac,short isOn)
        {
            var res = await hardwareService.SetTransactionAsync(mac,isOn);
            return Ok(res);
        }
    }
}
