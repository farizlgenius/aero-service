using HIDAeroService.Constants;
using HIDAeroService.Dto;
using HIDAeroService.Dto.Interval;
using HIDAeroService.Dto.TimeZone;
using HIDAeroService.Helpers;
using HIDAeroService.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Net;

namespace HIDAeroService.Controllers.V1
{
    [Route("api/[controller]")]
    [ApiController]
    public sealed class intervalController : ControllerBase
    {
        private readonly IIntervalService _intervalService;
        public intervalController(IIntervalService intervalService)
        {
            _intervalService = intervalService;
        }

        [HttpGet]
        public async Task<ActionResult<BaseResponse<IEnumerable<IntervalDto>>>> GetAsync()
        {
            var res = await _intervalService.GetAsync();
            return StatusCode((int)res.Code, res);
        }

        [HttpPost]
        public async Task<ActionResult<IntervalDto>> CreateAsync(IntervalDto dto)
        {
            var res = await _intervalService.CreateAsync(dto);
            return StatusCode((int)res.Code, res);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<IntervalDto>> RemoveAsync(short id) 
        {
            var res = await _intervalService.RemoveAsync(id);
            return StatusCode((int)res.Code, res);
        }

        [HttpPut]
        public async Task<ActionResult<IntervalDto>> UpdateAsync(IntervalDto dto)
        {
            var res = await _intervalService.UpdateAsync(dto);
            return StatusCode((int)res.Code, res);
        }
    }
}
