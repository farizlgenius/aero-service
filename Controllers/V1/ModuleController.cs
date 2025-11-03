using HIDAeroService.DTO;
using HIDAeroService.DTO.Module;
using HIDAeroService.Entity;
using HIDAeroService.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace HIDAeroService.Controllers.V1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ModuleController(IModuleService sioService) : ControllerBase
    {



        [HttpGet]
        public async Task<ActionResult<ResponseDto<IEnumerable<ModuleDto>>>> GetAsync()
        {
            var res = await sioService.GetAsync();
            return Ok(res);
        }

        [HttpGet("{mac}/{component}")]
        public async Task<ActionResult<ResponseDto<ModuleDto>>> GetByComponentAsync(string mac,short component)
        {
            var res = await sioService.GetByComponentAsync(mac,component);
            return Ok(res);
        }

        [HttpGet("{mac}")]
        public async Task<ActionResult<ResponseDto<IEnumerable<ModuleDto>>>> GetByMacAsync(string mac)
        {
            var res = await sioService.GetByMacAsync(mac);
            return Ok(res);
        }

        [HttpPost]
        public async Task<ActionResult<ResponseDto<ModuleDto>>> CreateAsync([FromBody] ModuleDto dto)
        {
            var res = await sioService.CreateAsync(dto);
            return Ok(res);
        }

        [HttpPut]
        public async Task<ActionResult<ResponseDto<ModuleDto>>> UpdateAsync([FromBody] ModuleDto dto)
        {
            var res = await sioService.UpdateAsync(dto);
            return Ok(res);
        }

        [HttpDelete("{mac}/{component}")]
        public async Task<ActionResult<ResponseDto<ModuleDto>>> DeleteAsync(string mac,short component)
        {
            var res = await sioService.DeleteAsync(mac,component);
            return Ok(res);
        }

        [HttpGet("status/{mac}/{component}")]
        public async Task<ActionResult<ResponseDto<bool>>> GetStatusAsync(string mac, short component)
        {
            var res = await sioService.GetStatusAsync(mac, component);
            return Ok(res);
        }
    }
}
