using HIDAeroService.DTO;
using HIDAeroService.DTO.CardHolder;
using HIDAeroService.Entity;
using HIDAeroService.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HIDAeroService.Controllers.V1
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

        [HttpDelete("{UserId}")]
        public async Task<ActionResult<ResponseDto<CardHolderDto>>> DeleteAsync(string UserId)
        {
            var res = await cardHolderService.DeleteAsync(UserId);
            return Ok(res);
        }

        [HttpGet]
        public async Task<ActionResult<ResponseDto<IEnumerable<CardHolderDto>>>> GetAsync()
        {
            var res = await cardHolderService.GetAsync();
            return Ok(res);
        }

        [HttpGet("{UserId}")]
        public async Task<ActionResult<ResponseDto<CardHolderDto>>> GetByComponentAsync(string UserId)
        {
            var res = await cardHolderService.GetByUserIdAsync(UserId);
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
