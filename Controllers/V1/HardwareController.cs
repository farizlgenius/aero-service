
using Microsoft.AspNetCore.Mvc;
using HIDAeroService.Data;
using HIDAeroService.Entity;
using HIDAeroService.Service;
using HIDAeroService.DTO;
using HIDAeroService.DTO.Scp;
using HIDAeroService.DTO.Hardware;
using AeroService.DTO.Hardware;
using Microsoft.AspNetCore.Authorization;

namespace HIDAeroService.Controllers.V1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class HardwareController(IHardwareService service) : ControllerBase
    {


        [HttpGet]
        [Authorize]
        public async Task<ActionResult<ResponseDto<IEnumerable<HardwareDto>>>> GetAsync()
        {
            var res = await service.GetAsync();
            return Ok(res);
        }

        [HttpGet("/api/v1/{location}/[controller]")]
        [Authorize]
        public async Task<ActionResult<ResponseDto<IEnumerable<HardwareDto>>>> GetByLocationAsync(short location)
        {
            var res = await service.GetAsync();
            return Ok(res);
        }

        [HttpGet("{mac}")]
        [Authorize]
        public async Task<ActionResult<ResponseDto<HardwareDto>>> GetByMacAsync(string mac)
        {
            var res = await service.GetByMacAsync(mac);
            return StatusCode((int)res.code, res);
        }


        [HttpGet("status/{mac}")]
        [Authorize]
        public async Task<ActionResult<ResponseDto<HardwareStatus>>> GetStatusAsync(string mac)
        {
            var res = await service.GetStatusAsync(mac);
            return Ok(res);
        }

        [HttpGet("tran/{mac}")]
        [Authorize]
        public async Task<ActionResult<ResponseDto<bool>>> GetTransactionLogStatusAsync(string mac)
        {
            var res = await service.GetTransactionLogStatusAsync(mac);
            return Ok(res);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<ResponseDto<HardwareDto>>> CreateAsync([FromBody] CreateHardwareDto dto)
        {
            var res = await service.CreateAsync(dto);
            return Ok(res);
        }

        [HttpPut]
        [Authorize]
        public async Task<ActionResult<ResponseDto<HardwareDto>>> UpdateAsync([FromBody] HardwareDto dto)
        {
            var res = await service.UpdateAsync(dto);
            return Ok(res);
        }

        [HttpDelete("{mac}")]
        [Authorize]
        public async Task<ActionResult<ResponseDto<bool>>> DeleteAsync(string mac)
        {
            var res = await service.DeleteAsync(mac);
            return Ok(res);
        }


        [HttpPost("reset/{mac}")]
        [Authorize]
        public async Task<ActionResult<ResponseDto<bool>>> ResetByIdAsync(string mac)
        {
            var res = await service.ResetAsync(mac);
            return Ok(res);
        }

        [HttpPost("reset/id/{id}")]
        [Authorize]
        public async Task<ActionResult<ResponseDto<bool>>> ResetByIdAsync(short id)
        {
            var res = await service.ResetAsync(id);
            return Ok(res);
        }



        [HttpPost("upload/{mac}")]
        [Authorize]
        public async Task<ActionResult<ResponseDto<bool>>> UploadConfigAsync(string mac)
        {
            var res = await service.UploadComponentConfigurationAsync(mac);
            return Ok(res);
        }


        [HttpPost("verify/mem/{mac}")]
        [Authorize]
        public async Task<ActionResult<ResponseDto<bool>>> VerifyMemoryAllocateAsync(string mac)
        {
            var res = await service.VerifyMemoryAllocateAsyncWithResponse(mac);
            return Ok(res);
        }

        [HttpPost("verify/com/{mac}")]
        [Authorize]
        public async Task<ActionResult<ResponseDto<bool>>> VerifyComponentConfigurationAsync(string mac)
        {
            var res = await service.VerifyComponentConfigurationAsync(mac);
            return Ok(res);
        }

        [HttpDelete("{mac}/{component}")]
        [Authorize]
        public Task<ActionResult<ResponseDto<HardwareDto>>> DeleteAsync(string mac, short component)
        {
            throw new NotImplementedException();
        }

        [HttpPost("tran/{mac}/{parameter}")]
        [Authorize]
        public async Task<ActionResult<ResponseDto<bool>>> SetTransactionAsync(string mac,short parameter)
        {
            var res = await service.SetTransactionAsync(mac, parameter);
            return Ok(res);
        }

        [HttpPost("tran/range")]
        [Authorize]
        public async Task<ActionResult<ResponseDto<bool>>> SetRangeTransactionAsync(List<SetTranDto> dtos)
        {
            var res = await service.SetRangeTransactionAsync(dtos);
            return Ok(res);
        }

        [HttpGet("type")]
        [Authorize]
        public async Task<ActionResult<ResponseDto<IEnumerable<ModeDto>>>> GetHardwareTypeAsync()
        {
            var res = await service.GetHardwareTypeAsync();
            return Ok(res);
        }


    }
}
