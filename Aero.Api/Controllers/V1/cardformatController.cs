using Aero.Application.DTOs;
using Aero.Application.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Aero.Api.Controllers.V1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CardFormatController(ICardFormatService cardFormatService) : ControllerBase
    {


        [HttpGet]
        public async Task<ActionResult<ResponseDto<IEnumerable<CardFormatDto>>>> GetAsync()
        {
            var res = await cardFormatService.GetAsync();
            return Ok(res);
        }

        [HttpGet("{component}")]
        public async Task<ActionResult<ResponseDto<CardFormatDto>>> GetByComponentAsync(short component)
        {
            var res = await cardFormatService.GetByComponentIdAsync(component);
            return Ok(res);
        }

        [HttpPost]
        public async Task<ActionResult<ResponseDto<bool>>> CreateAsync([FromBody] CardFormatDto dto)
        {
            var res = await cardFormatService.CreateAsync(dto);
            return Ok(res);
        }

        [HttpPut]
        public async Task<ActionResult<ResponseDto<CardFormatDto>>> UpdateAsync([FromBody] CardFormatDto dto)
        {
            var res = await cardFormatService.UpdateAsync(dto);
            return Ok(res);
        }

        [HttpDelete("{component}")]
        public async Task<ActionResult<ResponseDto<bool>>> DeleteAsync(short component)
        {
            var res = await cardFormatService.DeleteAsync(component);
            return Ok(res);
        }

    }
}
