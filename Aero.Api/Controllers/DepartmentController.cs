using Aero.Application.DTOs;
using Aero.Application.Interfaces;
using Aero.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Aero.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController(IDepartmentService service) : ControllerBase
    {
         [HttpGet]
        [Authorize]
        public async Task<ActionResult<ResponseDto<IEnumerable<DepartmentDto>>>> GetAsync()
        {
            var res = await service.GetAsync();
            return Ok(res);
        }

        [HttpGet("/api/{location}/[controller]")]
        [Authorize]
        public async Task<ActionResult<ResponseDto<IEnumerable<DepartmentDto>>>> GetPaginationAsync([FromQuery] PaginationParamsWithFilter param,int location)
        {
            var res = await service.GetPaginationAsync(param,location);
            return Ok(res); 
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<ResponseDto<bool>>> CreateAsync([FromBody] DepartmentDto dto)
        {
            var res = await service.CreateAsync(dto);
            return Ok(res);
        }

        [HttpPut]
        [Authorize]
        public async Task<ActionResult<ResponseDto<DepartmentDto>>> UpdateAsync([FromBody] DepartmentDto dto) 
        {
            var res = await service.UpdateAsync(dto);
            return Ok(res);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult<ResponseDto<DepartmentDto>>> DeleteByIdAsync(int id)
        {
            var res = await service.DeleteByIdAsync(id);
            return Ok(res);
        }

        [HttpPost("delete/range")]
        public async Task<ActionResult<ResponseDto<IEnumerable<DepartmentDto>>>> DeleteRangeAsync([FromBody] List<int> ids)
        {
            var res = await service.DeleteRangeAsync(ids);
            return Ok(res);
        }
    }
}
