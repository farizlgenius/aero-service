using Aero.Application.DTOs;
using Aero.Application.Interface;
using Aero.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aero.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ModuleController(IModuleService service) : ControllerBase
    {



        [HttpGet]
        [Authorize]
        public async Task<ActionResult<ResponseDto<IEnumerable<ModuleDto>>>> GetAsync()
        {
            var res = await service.GetAsync();
            return Ok(res);
        }

        [HttpGet("/api/{location}/[controller]")]
        [Authorize]
        public async Task<ActionResult<ResponseDto<IEnumerable<ModuleDto>>>> GetByLocationAsync(short location)
        {
            var res = await service.GetByLocationAsync(location);
            return Ok(res);
        }

        [HttpGet("/api/{location}/[controller]/pagination")]
        [Authorize]
        public async Task<ActionResult<ResponseDto<Pagination<ModuleDto>>>> GetPaginationAsync([FromQuery] PaginationParamsWithFilter param,short location)
        {
            var res = await service.GetPaginationAsync(param,location);
            return Ok(res);
        }

        [HttpGet("{mac}/{component}")]
        [Authorize]
        public async Task<ActionResult<ResponseDto<ModuleDto>>> GetByComponentAsync(string mac,short component)
        {
            var res = await service.GetByComponentAsync(mac,component);
            return Ok(res);
        }

        [HttpGet("{device}")]
        [Authorize]
        public async Task<ActionResult<ResponseDto<IEnumerable<ModuleDto>>>> GetByDeviceIdAsync(int device)
        {
            var res = await service.GetByDeviceIdAsync(device);
            return Ok(res);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<ResponseDto<ModuleDto>>> CreateAsync([FromBody] ModuleDto dto)
        {
            var res = await service.CreateAsync(dto);
            return Ok(res);
        }

        [HttpPut]
        [Authorize]
        public async Task<ActionResult<ResponseDto<ModuleDto>>> UpdateAsync([FromBody] ModuleDto dto)
        {
            var res = await service.UpdateAsync(dto);
            return Ok(res);
        }

        [HttpDelete("{mac}/{component}")]
        [Authorize]
        public async Task<ActionResult<ResponseDto<ModuleDto>>> DeleteAsync(string mac,short component)
        {
            var res = await service.DeleteAsync(mac,component);
            return Ok(res);
        }

        [HttpGet("status/{device}/{driver}")]
        [Authorize]
        public async Task<ActionResult<ResponseDto<bool>>> GetStatusAsync(int device, short driver)
        {
            var res = await service.GetStatusAsync(device, driver);
            return Ok(res);
        }

        [HttpGet("baudrate")]
        [Authorize]
        public async Task<ActionResult<ResponseDto<IEnumerable<Mode>>>> GetBaudrateAsync()
        {
            var res = await service.GetBaudrateAsync();
            return Ok(res);
        }

        [HttpGet("protocol")]
        [Authorize]
        public async Task<ActionResult<ResponseDto<IEnumerable<Mode>>>> GetProtocolAsync()
        {
            var res = await service.GetProtocolAsync();
            return Ok(res);
        }
    }
}
