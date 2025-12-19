using HIDAeroService.DTO;
using HIDAeroService.DTO.Role;
using HIDAeroService.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Crypto;

namespace HIDAeroService.Controllers.V1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class RoleController(IRoleService roleService) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<ResponseDto<IEnumerable<RoleDto>>>> GetAsync()
        {
            var res = await roleService.GetAsync();
            return Ok(res);
        }

        [HttpPost]
        public async Task<ActionResult<ResponseDto<bool>>> CreateAsync([FromBody] RoleDto dto)
        {
            var res = await roleService.CreateAsync(dto);
            return Ok(res);
        }

        [HttpPost("delete/range")]
        public async Task<ActionResult<ResponseDto<IEnumerable<ResponseDto<bool>>>>> DeleteRangeAsync([FromBody] List<short> dtos)
        {
            var res = await roleService.DeleteRangeAsync(dtos);
            return Ok(res);
        }

        [HttpPut]
        public async Task<ActionResult<ResponseDto<RoleDto>>> UpdateAsync([FromBody] RoleDto dto)
        {
            var res = await roleService.UpdateAsync(dto);
            return Ok(res);
        }

        [HttpDelete("{component}")]
        [Authorize]
        public async Task<ActionResult<ResponseDto<bool>>> DeleteByIdAsync(short component)
        {
            var res = await roleService.DeleteByComponentIdAsync(component);
            return Ok(res);
        }

        [HttpGet("{component}")]
        public async Task<ActionResult<ResponseDto<RoleDto>>> GetByIdAsync(short component)
        {
            var res = await roleService.GetByComponentIdAsync(component);
            return Ok(res);
        }


    }
}
