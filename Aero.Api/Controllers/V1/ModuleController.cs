using AeroService.DTO;
using AeroService.DTO.Module;
using AeroService.Entity;
using AeroService.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Aero.Api.Controllers.V1
{
    [Route("api/v1/[controller]")]
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

        [HttpGet("/api/v1/{location}/[controller]")]
        [Authorize]
        public async Task<ActionResult<ResponseDto<IEnumerable<ModuleDto>>>> GetByLocationAsync(short location)
        {
            var res = await service.GetByLocationAsync(location);
            return Ok(res);
        }

        [HttpGet("{mac}/{component}")]
        [Authorize]
        public async Task<ActionResult<ResponseDto<ModuleDto>>> GetByComponentAsync(string mac,short component)
        {
            var res = await service.GetByComponentAsync(mac,component);
            return Ok(res);
        }

        [HttpGet("{mac}")]
        [Authorize]
        public async Task<ActionResult<ResponseDto<IEnumerable<ModuleDto>>>> GetByMacAsync(string mac)
        {
            var res = await service.GetByMacAsync(mac);
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

        [HttpGet("status/{mac}/{component}")]
        [Authorize]
        public async Task<ActionResult<ResponseDto<bool>>> GetStatusAsync(string mac, short component)
        {
            var res = await service.GetStatusAsync(mac, component);
            return Ok(res);
        }

        [HttpGet("baudrate")]
        [Authorize]
        public async Task<ActionResult<ResponseDto<IEnumerable<ModeDto>>>> GetBaudrateAsync()
        {
            var res = await service.GetBaudrateAsync();
            return Ok(res);
        }

        [HttpGet("protocol")]
        [Authorize]
        public async Task<ActionResult<ResponseDto<IEnumerable<ModeDto>>>> GetProtocolAsync()
        {
            var res = await service.GetProtocolAsync();
            return Ok(res);
        }
    }
}
