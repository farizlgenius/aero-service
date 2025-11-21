using HIDAeroService.DTO;
using HIDAeroService.DTO.Role;
using HIDAeroService.Service;
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

        [HttpPut]
        public async Task<ActionResult<ResponseDto<RoleDto>>> UpdateAsync([FromBody] RoleDto dto)
        {
            var res = await roleService.UpdateAsync(dto);
            return Ok(res);
        }

        [HttpDelete("{Id}")]
        public async Task<ActionResult<ResponseDto<bool>>> DeleteByIdAsync(short Id)
        {
            var res = await roleService.DeleteByComponentIdAsync(Id);
            return Ok(res);
        }

        [HttpGet("{Id}")]
        public async Task<ActionResult<ResponseDto<RoleDto>>> GetByIdAsync(short Id)
        {
            var res = await roleService.GetByComponentIdAsync(Id);
            return Ok(res);
        }


    }
}
