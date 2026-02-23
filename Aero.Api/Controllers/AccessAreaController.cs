using Aero.Application.DTOs;
using Aero.Application.Interface;
using Aero.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aero.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccessAreaController(IAccessAreaService accessareaService) : ControllerBase
    {
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<ResponseDto<IEnumerable<AccessAreaDto>>>> GetAsync()
        {
            var res = await accessareaService.GetAsync();
            return Ok(res);
        }

        [HttpGet("/api/{location}/[controller]")]
        [Authorize]
        public async Task<ActionResult<ResponseDto<IEnumerable<AccessAreaDto>>>> GetByLocationIdAsync(short location)
        {
            var res = await accessareaService.GetByLocationIdAsync(location);
            return Ok(res);
        }

        [HttpGet("/api/{location}/[controller]/pagination")]
        [Authorize]
        public async Task<ActionResult<ResponseDto<Pagination<AccessAreaDto>>>> GetPaginationAsync([FromQuery]PaginationParamsWithFilter param,short location)
        {
            var res = await accessareaService.GetPaginationAsync(param,location);
            return Ok(res);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<ResponseDto<bool>>> CreateAsync([FromBody] CreateAccessAreaDto dto) 
        {
            var res = await accessareaService.CreateAsync(dto);
            return Ok(res);
        }

        [HttpPut]
        [Authorize]
        public async Task<ActionResult<ResponseDto<AccessAreaDto>>> UpdateAsync([FromBody] AccessAreaDto dto)
        {
            var res = await accessareaService.UpdateAsync(dto);
            return Ok(res);
        }

        [HttpDelete("{component}")]
        [Authorize]
        public async Task<ActionResult<ResponseDto<bool>>> DeleteAsync(short component)
        {
            var res = await accessareaService.DeleteAsync(component);
            return Ok(res);
        }

        [HttpGet("{component}")]
        [Authorize]
        public async Task<ActionResult<ResponseDto<AccessAreaDto>>> GetByComponentAsync(short component)
        {
            var res = await accessareaService.GetByComponentAsync(component);
            return Ok(res);
        }

        [HttpGet("command")]
        [Authorize]
        public async Task<ActionResult<ResponseDto<Mode>>> GetCommandAsync()
        {
            var res = await accessareaService.GetCommandAsync();
            return Ok(res);
        }

        [HttpGet("access")]
        [Authorize]
        public async Task<ActionResult<ResponseDto<Mode>>> GetAccessControlOptionAsync()
        {
            var res = await accessareaService.GetAccessControlOptionAsync();
            return Ok(res);
        }


        [HttpGet("occcontrol")]
        [Authorize]
        public async Task<ActionResult<ResponseDto<Mode>>> GetOccupancyControlOptionAsync()
        {
            var res = await accessareaService.GetOccupancyControlOptionAsync();
            return Ok(res);
        }

        [HttpGet("areaflag")]
        [Authorize]
        public async Task<ActionResult<ResponseDto<Mode>>> GetAreaFlagOptionAsync()
        {
            var res = await accessareaService.GetAreaFlagOptionAsync();
            return Ok(res);
        }


        [HttpGet("multiocc")]
        [Authorize]
        public async Task<ActionResult<ResponseDto<Mode>>> GetMultiOccupancyOptionAsync()
        {
            var res = await accessareaService.GetMultiOccupancyOptionAsync();
            return Ok(res);
        }


    }
}
