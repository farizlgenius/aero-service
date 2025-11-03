using HIDAeroService.DTO;
using HIDAeroService.DTO.MonitorGroup;
using HIDAeroService.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HIDAeroService.Controllers.V1
{
    [Route("api/[controller]")]
    [ApiController]
    public class MonitorGroupController(IMonitorGroupService service) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<ResponseDto<IEnumerable<MonitorGroupDto>>>> GetAsync()
        {
            var res = await service.GetAsync();
            return Ok(res);
        }

        [HttpPost]
        public async Task<ActionResult<ResponseDto<MonitorGroupDto>>> CreateAsync(MonitorGroupDto dto)
        {
            var res = await service.CreateAsync(dto);
            return Ok(res);
        }

        [HttpDelete]
        public async Task<ActionResult<ResponseDto<MonitorGroupDto>>> DeleteAsync(string mac,short component)
        {
            var res = await service.DeleteAsync(mac,component);
            return Ok(res);
        }

        [HttpPut]
        public async Task<ActionResult<ResponseDto<MonitorGroupDto>>> UpdateAsync(MonitorGroupDto dto)
        {
            var res =  await service.UpdateAsync(dto);
            return Ok(res);
        }
    }
}
