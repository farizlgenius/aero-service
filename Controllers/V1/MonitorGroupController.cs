using HIDAeroService.DTO;
using HIDAeroService.DTO.MonitorGroup;
using HIDAeroService.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HIDAeroService.Controllers.V1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class MonitorGroupController(IMonitorGroupService service) : ControllerBase
    {
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<ResponseDto<IEnumerable<MonitorGroupDto>>>> GetAsync()
        {
            var res = await service.GetAsync();
            return Ok(res);
        }

        [HttpGet("/api/v1/{location}/[controller]")]
        [Authorize]
        public async Task<ActionResult<ResponseDto<IEnumerable<MonitorGroupDto>>>> GetByLocationIdAsync(short location)
        {
            var res = await service.GetByLocationAsync(location);
            return Ok(res);
        }

        [HttpGet("command")]
        [Authorize]
        public async Task<ActionResult<ResponseDto<ModeDto>>> GetCommandAsync()
        {
            var res = await service.GetCommandAsync();
            return Ok(res);
        }

        [HttpGet("type")]
        [Authorize]
        public async Task<ActionResult<ResponseDto<IEnumerable<ModeDto>>>> GetTypeAsync()
        {
            var res = await service.GetTypeAsync();
            return Ok(res);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<ResponseDto<MonitorGroupDto>>> CreateAsync([FromBody] MonitorGroupDto dto)
        {
            var res = await service.CreateAsync(dto);
            return Ok(res);
        }

        [HttpPost("command")]
        [Authorize]
        public async Task<ActionResult<ResponseDto<bool>>> MonitorGroupCommand([FromBody] MonitorGroupCommandDto dto )
        {
            var res = await service.MonitorGroupCommandAsync(dto);
            return Ok(res);
        }

        [HttpDelete("{mac}/{component}")]
        [Authorize]
        public async Task<ActionResult<ResponseDto<MonitorGroupDto>>> DeleteAsync(string mac,short component)
        {
            var res = await service.DeleteAsync(mac,component);
            return Ok(res);
        }

        [HttpPut]
        [Authorize]
        public async Task<ActionResult<ResponseDto<MonitorGroupDto>>> UpdateAsync(MonitorGroupDto dto)
        {
            var res =  await service.UpdateAsync(dto);
            return Ok(res);
        }
    }
}
