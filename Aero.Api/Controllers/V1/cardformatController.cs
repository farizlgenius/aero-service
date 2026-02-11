using Aero.Application.DTOs;
using Aero.Application.Interface;
using Aero.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aero.Api.Controllers.V1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CardFormatController(ICardFormatService cardFormatService) : ControllerBase
    {


        [HttpGet]
        [Authorize]
        public async Task<ActionResult<ResponseDto<IEnumerable<CardFormatDto>>>> GetAsync()
        {
            var res = await cardFormatService.GetAsync();
            return Ok(res);
        }

        [HttpGet("/api/v1/{location}/[controller]")]
        [Authorize]
        public async Task<ActionResult<ResponseDto<IEnumerable<CardFormatDto>>>> GetByLocationIdAsync(short location)
        {
            var res = await cardFormatService.GetAsync();
            return Ok(res);
        }

        [HttpGet("/api/v1/{location}/[controller]/pagination")]
        [Authorize]
        public async Task<ActionResult<ResponseDto<Pagination<CardFormatDto>>>> GetPaginationAsync([FromQuery] PaginationParamsWithFilter param,short location)
        {
            var res = await cardFormatService.GetPaginationAsync(param,location);
            return Ok(res);
        }

        [HttpGet("{component}")]
        [Authorize]
        public async Task<ActionResult<ResponseDto<CardFormatDto>>> GetByComponentAsync(short component)
        {
            var res = await cardFormatService.GetByComponentIdAsync(component);
            return Ok(res);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<ResponseDto<bool>>> CreateAsync([FromBody] CardFormatDto dto)
        {
            var res = await cardFormatService.CreateAsync(dto);
            return Ok(res);
        }

        [HttpPut]
        [Authorize]
        public async Task<ActionResult<ResponseDto<CardFormatDto>>> UpdateAsync([FromBody] CardFormatDto dto)
        {
            var res = await cardFormatService.UpdateAsync(dto);
            return Ok(res);
        }

        [HttpDelete("{component}")]
        [Authorize]
        public async Task<ActionResult<ResponseDto<bool>>> DeleteAsync(short component)
        {
            var res = await cardFormatService.DeleteAsync(component);
            return Ok(res);
        }

    }
}
