

using Aero.Application.DTOs;
using Aero.Application.Interface;
using Aero.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aero.Api.Controllers.V1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public sealed class AccessLevelController(IAccessLevelService accesslevelService) : ControllerBase
    {

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<ResponseDto<IEnumerable<AccessLevelDto>>>> GetAsync()
        {
            var res = await accesslevelService.GetAsync();
            return Ok(res);
        }

        [HttpGet("/api/v1/{location}/[controller]")]
        [Authorize]
        public async Task<ActionResult<ResponseDto<IEnumerable<AccessLevelDto>>>> GetByLocationIdAsync(short location)
        {
            var res = await accesslevelService.GetByLocationIdAsync(location);
            return Ok(res);
        }

        [HttpGet("/api/v1/{location}/[controller]/pagination")]
        [Authorize]
        public async Task<ActionResult<ResponseDto<Pagination<AccessLevelDto>>>> GetPaginationAsync([FromQuery]PaginationParamsWithFilter param,short location)
        {
            var res = await accesslevelService.GetPaginationAsync(param,location);
            return Ok(res);
        }

        [HttpGet("{component}")]
        [Authorize]
        public async Task<ActionResult<ResponseDto<AccessLevelDto>>> GetByComponentAsync(short component)
        {
            var  res = await accesslevelService.GetByComponentIdAsync(component);
            return Ok(res);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<ResponseDto<AccessLevelDto>>> CreateAsync([FromBody] CreateUpdateAccessLevelDto dto)
        {
            var res = await accesslevelService.CreateAsync(dto);
            return Ok(res);
        }

        [HttpDelete("{component}")]
        [Authorize]
        public async Task<ActionResult<ResponseDto<AccessLevelDto>>> DeleteAsync(short component)
        {
            var res = await accesslevelService.DeleteAsync(component);
            return Ok(res);
        }

        [HttpPut]
        [Authorize]
        public async Task<ActionResult<ResponseDto<AccessLevelDto>>> UpdateAsync([FromBody] CreateUpdateAccessLevelDto dto)
        {
            var res = await accesslevelService.UpdateAsync(dto);
            return Ok(res);
        }
    }
}
