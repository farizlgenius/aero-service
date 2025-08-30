using HIDAeroService.Constants;
using HIDAeroService.Dto;
using HIDAeroService.Dto.CardFormat;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HIDAeroService.Helpers;
using HIDAeroService.Service.Interface;
using System.Net;

namespace HIDAeroService.Controllers.V1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class cardformatController : ControllerBase
    {
        private readonly ICardFormatService _cardFormatService;

        public cardformatController(ICardFormatService cardFormatService)
        {
            _cardFormatService = cardFormatService;
        }

        [HttpGet("all")]
        public async Task<ActionResult<BaseResponse<IEnumerable<CardFormatDto>>>> GetAll()
        {
            var data = await _cardFormatService.GetAll();
            if (data.Count() > 0)
            {
                return Helper.ResponseBuilder<IEnumerable<CardFormatDto>>(HttpStatusCode.OK,Constants.ConstantsHelper.SUCCESS, data);
            }
            return Helper.ResponseBuilder<IEnumerable<CardFormatDto>>(HttpStatusCode.NoContent, Constants.ConstantsHelper.NOT_FOUND_RECORD,Enumerable.Empty<CardFormatDto>());
        }

        [HttpPost("add")]
        public async Task<ActionResult<BaseResponse<CardFormatDto>>> Create([FromBody] CreateCardFormatDto dto ) 
        {
            var data = await _cardFormatService.Add(dto);
            if (data == null)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError,Helper.ResponseBuilder(HttpStatusCode.InternalServerError,ConstantsHelper.INTERNAL_ERROR));
            }
            return Ok(Helper.ResponseBuilder<CardFormatDto>(HttpStatusCode.Created,ConstantsHelper.CREATED,data));
        }

        [HttpDelete("remove/{cardFormatNo}")]
        public async Task<ActionResult<BaseDto<CardFormatDto>>> Delete(short cardFormatNo) 
        {
            var content = await _cardFormatService.Delete(cardFormatNo);
            if (content == null)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, Helper.ResponseBuilder(HttpStatusCode.InternalServerError,ConstantsHelper.INTERNAL_ERROR));
            }
            return Ok(Helper.ResponseBuilder<CardFormatDto>(HttpStatusCode.OK,ConstantsHelper.SUCCESS, content));
        }
    }
}
