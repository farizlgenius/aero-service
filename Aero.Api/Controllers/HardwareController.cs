
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Aero.Application.Interface;
using Aero.Application.DTOs;
using Aero.Domain.Entities;

namespace Aero.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HardwareController(IDeviceService service) : ControllerBase
    {


        [HttpGet]
        [Authorize]
        public async Task<ActionResult<ResponseDto<IEnumerable<DeviceDto>>>> GetAsync()
        {
            var res = await service.GetAsync();
            return Ok(res);
        }

        [HttpGet("/api/{location}/[controller]")]
        [Authorize]
        public async Task<ActionResult<ResponseDto<IEnumerable<DeviceDto>>>> GetByLocationAsync(short location)
        {
            var res = await service.GetByLocationAsync(location);
            return Ok(res);
        }

        [HttpGet("/api/{location}/[controller]/pagination")]
        [Authorize]
        public async Task<ActionResult<ResponseDto<Pagination<DeviceDto>>>> GetPaginationAsync([FromQuery] PaginationParamsWithFilter param,short location)
        {
            var res = await service.GetPaginationAsync(param,location);
            return Ok(res);
        }

        [HttpGet("{mac}")]
        [Authorize]
        public async Task<ActionResult<ResponseDto<DeviceDto>>> GetByMacAsync(string mac)
        {
            var res = await service.GetByMacAsync(mac);
            return StatusCode((int)res.code, res);
        }


        [HttpGet("status/{id}")]
        [Authorize]
        public async Task<ActionResult<ResponseDto<DeviceStatusDto>>> GetStatusAsync(int id)
        {
            var res = await service.GetStatusAsync(id);
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
        public async Task<ActionResult<ResponseDto<DeviceDto>>> CreateAsync([FromBody] CreateDeviceDto dto)
        {
            var res = await service.CreateAsync(dto);
            return Ok(res);
        }

        [HttpPut]
        [Authorize]
        public async Task<ActionResult<ResponseDto<DeviceDto>>> UpdateAsync([FromBody] DeviceDto dto)
        {
            var res = await service.UpdateAsync(dto);
            return Ok(res);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult<ResponseDto<bool>>> DeleteAsync(int id)
        {
            var res = await service.DeleteAsync(id);
            return Ok(res);
        }


        [HttpPost("reset/{mac}")]
        [Authorize]
        public async Task<ActionResult<ResponseDto<bool>>> ResetByIdAsync(string mac)
        {
            var res = await service.ResetByMacAsync(mac);
            return Ok(res);
        }

        [HttpPost("reset/id/{id}")]
        [Authorize]
        public async Task<ActionResult<ResponseDto<bool>>> ResetByIdAsync(short id)
        {
            var res = await service.ResetByComponentAsync(id);
            return Ok(res);
        }



        [HttpPost("upload/{id}")]
        [Authorize]
        public async Task<ActionResult<ResponseDto<bool>>> UploadConfigAsync(int id)
        {
            var res = await service.UploadComponentConfigurationAsync(id);
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
        public Task<ActionResult<ResponseDto<DeviceDto>>> DeleteAsync(string mac, short component)
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
        public async Task<ActionResult<ResponseDto<IEnumerable<Mode>>>> GetHardwareTypeAsync()
        {
            var res = await service.GetHardwareTypeAsync();
            return Ok(res);
        }


    }
}
