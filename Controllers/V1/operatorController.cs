using HIDAeroService.DTO;
using HIDAeroService.DTO.Operator;
using HIDAeroService.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HIDAeroService.Controllers.V1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class OperatorController(IOperatorService operatorService) : ControllerBase
    {
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<ResponseDto<IEnumerable<OperatorDto>>>> GetAsync()
        {
            var res = await operatorService.GetAsync();
            return Ok(res);
        }

        [HttpGet("/api/v1/{location}/[controller]")]
        [Authorize]
        public async Task<ActionResult<ResponseDto<IEnumerable<OperatorDto>>>> GetByLocationAsync(short location)
        {
            var res = await operatorService.GetByLocationAsync(location);
            return Ok(res);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<ResponseDto<bool>>> CreateAsync([FromBody] CreateOperatorDto dto)
        {
            var res = await operatorService.CreateAsync(dto);
            return Ok(res);
        }

        [HttpPut]
        [Authorize]
        public async Task<ActionResult<ResponseDto<CreateOperatorDto>>> UpdateAsync([FromBody] CreateOperatorDto dto)
        {
            var res = await operatorService.UpdateAsync(dto);
            return Ok(res);
        }

        [HttpDelete("{component}")]
        [Authorize]
        public async Task<ActionResult<ResponseDto<bool>>> DeleteByIdAsync(short component)
        {
            var res = await operatorService.DeleteByIdAsync(component);
            return Ok(res);
        }

        [HttpPost("delete/range")]
        public async Task<ActionResult<ResponseDto<IEnumerable<ResponseDto<bool>>>>> DeleteRangeAsync([FromBody] List<short> dtos) 
        {
            var res = await operatorService.DeleteRangeAsync(dtos);
            return Ok(res);
        }

        [HttpGet("{username}")]
        [Authorize]
        public async Task<ActionResult<ResponseDto<OperatorDto>>> GetByUsernameAsync(string username)
        {

            var res = await operatorService.GetByUsernameAsync(username);
            return Ok(res);
        }

        [HttpPut("password/update")]
        [Authorize]
        public async Task<ActionResult<ResponseDto<bool>>> UpdatePasswordAsync(PasswordDto dto)
        {
            var res = await operatorService.UpdatePasswordAsync(dto);
            return Ok(res);
        }


    }
}
