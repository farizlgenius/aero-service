
using Aero.Application.DTOs;
using Aero.Application.Interface;
using Aero.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aero.Api.Controllers.V1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public sealed class IntervalController(IIntervalService service) : ControllerBase
    {

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<ResponseDto<IEnumerable<IntervalDto>>>> GetAsync()
        {
            var res = await service.GetAsync();
            return Ok(res);
        }

        [HttpGet("/api/v1/{location}/[controller]")]
        [Authorize]
        public async Task<ActionResult<ResponseDto<IEnumerable<IntervalDto>>>> GetByLocationAsync(short location)
        {
            var res = await service.GetByLocationAsync(location);
            return Ok(res);
        }

        [HttpGet("/api/v1/{location}/[controller]/pagination")]
        [Authorize]
        public async Task<ActionResult<ResponseDto<Pagination<IntervalDto>>>> GetPaginationAsync([FromQuery] PaginationParamsWithFilter param, short location)
        {
            var res = await service.GetPaginationAsync(param,location);
            return Ok(res);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<ResponseDto<IntervalDto>>> CreateAsync([FromBody] IntervalDto dto)
        {
            var res = await service.CreateAsync(dto);
            return Ok(res);
        }

        [HttpDelete("{component}")]
        public async Task<ActionResult<ResponseDto<IntervalDto>>> DeleteAsync(short component) 
        {
            var res = await service.DeleteAsync(component);
            return Ok(res);
        }

        [HttpPost("delete/range")]
        [Authorize]
        public async Task<ActionResult<ResponseDto<IEnumerable<ResponseDto<bool>>>>> DeleteRangeAsync([FromBody] List<short> components)
        {
            var res = await service.DeleteRangeAsync(components);
            return Ok(res);
        }

        [HttpPut]
        [Authorize]
        public async Task<ActionResult<ResponseDto<IntervalDto>>> UpdateAsync([FromBody] IntervalDto dto)
        {
            var res = await service.UpdateAsync(dto);
            return Ok(res);
        }

        [HttpGet("{component}")]
        [Authorize]
        public async Task<ActionResult<ResponseDto<IntervalDto>>> GetByComponentAsync(short component)
        {
            var res = await service.GetByIdAsync(component);
            return Ok(res);
        }
    }
}
