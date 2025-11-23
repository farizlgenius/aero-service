using HIDAeroService.DTO;
using HIDAeroService.DTO.Operator;
using HIDAeroService.Service;
using Microsoft.AspNetCore.Mvc;

namespace HIDAeroService.Controllers.V1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class OperatorController(IOperatorService operatorService) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<ResponseDto<IEnumerable<OperatorDto>>>> GetAsync()
        {
            var res = await operatorService.GetAsync();
            return Ok(res);
        }

        [HttpPost]
        public async Task<ActionResult<ResponseDto<bool>>> CreateAsync([FromBody] CreateOperatorDto dto)
        {
            var res = await operatorService.CreateAsync(dto);
            return Ok(res);
        }

        [HttpPut]
        public async Task<ActionResult<ResponseDto<CreateOperatorDto>>> UpdateAsync([FromBody] CreateOperatorDto dto)
        {
            var res = await operatorService.UpdateAsync(dto);
            return Ok(res);
        }

        [HttpDelete("{Username}")]
        public async Task<ActionResult<ResponseDto<bool>>> DeleteByUsernameAsync(string Username)
        {
            var res = await operatorService.DeleteByUsernameAsync(Username);
            return Ok(res);
        }

        [HttpGet("{Username}")]
        public async Task<ActionResult<ResponseDto<OperatorDto>>> GetByUsernameAsync(string Username)
        {

            var res = await operatorService.GetByUsernameAsync(Username);
            return Ok(res);
        }
    }
}
