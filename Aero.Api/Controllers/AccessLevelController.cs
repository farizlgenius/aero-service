using Aero.Api.Authorization;
using Aero.Application.DTOs;
using Aero.Application.Interface;
using Aero.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aero.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public sealed class AccessLevelController(IAccessLevelService accesslevelService) : ControllerBase
    {

        [HttpGet]
        [Scope("level.read")]
        public async Task<ActionResult<ResponseDto<IEnumerable<AccessLevelDto>>>> GetAsync()
        {
            var res = await accesslevelService.GetAsync();
            return Ok(res);
        }

        [HttpGet("/api/{location}/[controller]")]
        [Scope("level.read")]
        public async Task<ActionResult<ResponseDto<IEnumerable<AccessLevelDto>>>> GetByLocationIdAsync(short location)
        {
            var res = await accesslevelService.GetByLocationIdAsync(location);
            return Ok(res);
        }

        [HttpGet("/api/{location}/[controller]/pagination")]
        [Scope("level.read")]
        public async Task<ActionResult<ResponseDto<Pagination<AccessLevelDto>>>> GetPaginationAsync([FromQuery]PaginationParamsWithFilter param,short location)
        {
            var res = await accesslevelService.GetPaginationAsync(param,location);
            return Ok(res);
        }

        [HttpGet("{component}")]
        [Scope("level.read")]
        public async Task<ActionResult<ResponseDto<AccessLevelDto>>> GetByComponentAsync(short component)
        {
            var  res = await accesslevelService.GetByIdAsync(component);
            return Ok(res);
        }

        [HttpPost]
        [Scope("level.create")]
        public async Task<ActionResult<ResponseDto<AccessLevelDto>>> CreateAsync([FromBody] CreateAccessLevelDto dto)
        {
            var res = await accesslevelService.CreateAsync(dto);
            return Ok(res);
        }

        [HttpDelete("{component}")]
        [Scope("level.delete")]
        public async Task<ActionResult<ResponseDto<AccessLevelDto>>> DeleteAsync(short component)
        {
            var res = await accesslevelService.DeleteAsync(component);
            return Ok(res);
        }

        [HttpPut]
        [Scope("level.update")]
        public async Task<ActionResult<ResponseDto<AccessLevelDto>>> UpdateAsync([FromBody] AccessLevelDto dto)
        {
            var res = await accesslevelService.UpdateAsync(dto);
            return Ok(res);
        }
    }
}
