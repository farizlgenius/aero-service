using AeroService.DTO;
using AeroService.DTO.CardHolder;
using AeroService.Entity;
using AeroService.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AeroService.Controllers.V1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CardHolderController(ICardHolderService cardHolderService) : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<ResponseDto<bool>>> CreateAsync([FromBody] CardHolderDto dto)
        {
            var res = await cardHolderService.CreateAsync(dto);
            return Ok(res);
        }

        [HttpDelete("{userid}")]
        public async Task<ActionResult<ResponseDto<CardHolderDto>>> DeleteAsync(string userid)
        {
            var res = await cardHolderService.DeleteAsync(userid);
            return Ok(res);
        }

        [HttpGet]
        public async Task<ActionResult<ResponseDto<IEnumerable<CardHolderDto>>>> GetAsync()
        {
            var res = await cardHolderService.GetAsync();
            return Ok(res);
        }

        [HttpGet("/api/v1/{location}/[controller]")]
        public async Task<ActionResult<ResponseDto<IEnumerable<CardHolderDto>>>> GetByLocationIdAsync(short location)
        {
            var res = await cardHolderService.GetByLocationIdAsync(location);
            return Ok(res);
        }

        [HttpGet("{userid}")]
        public async Task<ActionResult<ResponseDto<CardHolderDto>>> GetByComponentAsync(string userid)
        {
            var res = await cardHolderService.GetByUserIdAsync(userid);
            return Ok(res);
        }

        [HttpPut]
        public async Task<ActionResult<ResponseDto<CardHolderDto>>> UpdateAsync([FromBody] CardHolderDto dto)
        {
            var res = await cardHolderService.UpdateAsync(dto);
            return Ok(res);
        }
    }
}
