
using Aero.Application.DTOs;
using Aero.Application.Interface;
using Aero.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aero.Api.Controllers.V1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class OperatorController(IOperatorService service) : ControllerBase
    {
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<ResponseDto<Pagination<OperatorDto>>>> GetAsync()
        {
            var res = await service.GetAsync();
            return Ok(res);
        }

        [HttpGet("/api/v1/{location}/[controller]")]
        [Authorize]
        public async Task<ActionResult<ResponseDto<IEnumerable<OperatorDto>>>> GetByLocationAsync(short location)
        {
            var res = await service.GetByLocationAsync(location);
            return Ok(res);
        }

        [HttpGet("/api/v1/{location}/[controller]/pagination")]
        [Authorize]
        public async Task<ActionResult<ResponseDto<Pagination<OperatorDto>>>> GetPaginationAsync([FromQuery] PaginationParamsWithFilter param, short location)
        {
            var res = await service.GetPaginationAsync(param, location);
            return Ok(res);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<ResponseDto<bool>>> CreateAsync([FromBody] CreateOperatorDto dto)
        {
            var res = await service.CreateAsync(dto);
            return Ok(res);
        }

        [HttpPut]
        [Authorize]
        public async Task<ActionResult<ResponseDto<CreateOperatorDto>>> UpdateAsync([FromBody] CreateOperatorDto dto)
        {
            var res = await service.UpdateAsync(dto);
            return Ok(res);
        }

        [HttpDelete("{component}")]
        [Authorize]
        public async Task<ActionResult<ResponseDto<bool>>> DeleteByIdAsync(short component)
        {
            var res = await service.DeleteByIdAsync(component);
            return Ok(res);
        }

        [HttpPost("delete/range")]
        public async Task<ActionResult<ResponseDto<IEnumerable<ResponseDto<bool>>>>> DeleteRangeAsync([FromBody] List<short> dtos) 
        {
            var res = await service.DeleteRangeAsync(dtos);
            return Ok(res);
        }

        [HttpGet("{username}")]
        [Authorize]
        public async Task<ActionResult<ResponseDto<OperatorDto>>> GetByUsernameAsync(string username)
        {

            var res = await service.GetByUsernameAsync(username);
            return Ok(res);
        }

        [HttpPut("password/update")]
        [Authorize]
        public async Task<ActionResult<ResponseDto<bool>>> UpdatePasswordAsync(PasswordDto dto)
        {
            var res = await service.UpdatePasswordAsync(dto);
            return Ok(res);
        }


    }
}
