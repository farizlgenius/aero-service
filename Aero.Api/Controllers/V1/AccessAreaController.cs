

using Aero.Application.DTOs;
using Aero.Application.Interface;
using Aero.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Aero.Api.Controllers.V1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AccessAreaController(IAccessAreaService accessareaService) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<ResponseDto<IEnumerable<AccessAreaDto>>>> GetAsync()
        {
            var res = await accessareaService.GetAsync();
            return Ok(res);
        }

        [HttpPost]
        public async Task<ActionResult<ResponseDto<bool>>> CreateAsync([FromBody] AccessAreaDto dto) 
        {
            var res = await accessareaService.CreateAsync(dto);
            return Ok(res);
        }

        [HttpPut]
        public async Task<ActionResult<ResponseDto<AccessAreaDto>>> UpdateAsync([FromBody] AccessAreaDto dto)
        {
            var res = await accessareaService.UpdateAsync(dto);
            return Ok(res);
        }

        [HttpDelete("{component}")]
        public async Task<ActionResult<ResponseDto<bool>>> DeleteAsync(short component)
        {
            var res = await accessareaService.DeleteAsync(component);
            return Ok(res);
        }

        [HttpGet("{component}")]
        public async Task<ActionResult<ResponseDto<AccessAreaDto>>> GetByComponentAsync(short component)
        {
            var res = await accessareaService.GetByComponentAsync(component);
            return Ok(res);
        }

        [HttpGet("command")]
        public async Task<ActionResult<ResponseDto<Mode>>> GetCommandAsync()
        {
            var res = await accessareaService.GetCommandAsync();
            return Ok(res);
        }

        [HttpGet("access")]
        public async Task<ActionResult<ResponseDto<Mode>>> GetAccessControlOptionAsync()
        {
            var res = await accessareaService.GetAccessControlOptionAsync();
            return Ok(res);
        }


        [HttpGet("occcontrol")]
        public async Task<ActionResult<ResponseDto<Mode>>> GetOccupancyControlOptionAsync()
        {
            var res = await accessareaService.GetOccupancyControlOptionAsync();
            return Ok(res);
        }

        [HttpGet("areaflag")]
        public async Task<ActionResult<ResponseDto<Mode>>> GetAreaFlagOptionAsync()
        {
            var res = await accessareaService.GetAreaFlagOptionAsync();
            return Ok(res);
        }


        [HttpGet("multiocc")]
        public async Task<ActionResult<ResponseDto<Mode>>> GetMultiOccupancyOptionAsync()
        {
            var res = await accessareaService.GetMultiOccupancyOptionAsync();
            return Ok(res);
        }


    }
}
