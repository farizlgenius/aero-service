using AeroService.DTO;
using AeroService.DTO.Role;
using AeroService.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Crypto;

namespace Aero.Api.Controllers.V1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class RoleController(IRoleService service) : ControllerBase
    {
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<ResponseDto<IEnumerable<RoleDto>>>> GetAsync()
        {
            var res = await service.GetAsync();
            return Ok(res);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<ResponseDto<bool>>> CreateAsync([FromBody] RoleDto dto)
        {
            var res = await service.CreateAsync(dto);
            return Ok(res);
        }

        [HttpPost("delete/range")]
        [Authorize]
        public async Task<ActionResult<ResponseDto<IEnumerable<ResponseDto<bool>>>>> DeleteRangeAsync([FromBody] List<short> dtos)
        {
            var res = await service.DeleteRangeAsync(dtos);
            return Ok(res);
        }

        [HttpPut]
        [Authorize]
        public async Task<ActionResult<ResponseDto<RoleDto>>> UpdateAsync([FromBody] RoleDto dto)
        {
            var res = await service.UpdateAsync(dto);
            return Ok(res);
        }

        [HttpDelete("{component}")]
        [Authorize]
        public async Task<ActionResult<ResponseDto<bool>>> DeleteByIdAsync(short component)
        {
            var res = await service.DeleteByComponentIdAsync(component);
            return Ok(res);
        }

        [HttpGet("{component}")]
        [Authorize]
        public async Task<ActionResult<ResponseDto<RoleDto>>> GetByIdAsync(short component)
        {
            var res = await service.GetByComponentIdAsync(component);
            return Ok(res);
        }


    }
}
