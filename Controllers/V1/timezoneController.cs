using HIDAeroService.Dto;
using HIDAeroService.Dto.TimeZone;
using HIDAeroService.Entity;
using HIDAeroService.Service.Impl;
using HIDAeroService.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace HIDAeroService.Controllers.V1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public sealed class timezoneController : ControllerBase
    {
        private readonly ITimeZoneService _timeZoneService;

        public timezoneController(ITimeZoneService timeZoneService)
        {
            _timeZoneService = timeZoneService;
        }

        [HttpGet]
        public async Task<ActionResult<Response<IEnumerable<TimeZoneDto>>>> GetAsync()
        {
            var res = await _timeZoneService.GetAsync();
            if (res.Errors.Count > 0) return StatusCode((int)res.Code, res);
            return Ok(res);
        }

        [HttpPost]
        public async Task<ActionResult<Response<TimeZoneDto>>> CreateAsync([FromBody] TimeZoneDto dto) 
        {
            var res = await _timeZoneService.CreateAsync(dto);
            if(res.Data == null) return StatusCode((int)res.Code,res);
            return StatusCode((int)res.Code,res);
        }

        [HttpPut]
        public async Task<ActionResult<Response<TimeZoneDto>>> UpdateAsync([FromBody] TimeZoneDto dto)
        {
            var res = await _timeZoneService.UpdateAsync(dto);
            if (res.Data == null) return StatusCode((int)HttpStatusCode.InternalServerError, res);
            return StatusCode((int)res.Code, res);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Response<TimeZoneDto>>> RemoveAsync(short id)
        {
            var res = await _timeZoneService.DeleteAsync(id);
            if (res.Data == null) return StatusCode((int)HttpStatusCode.InternalServerError, res);
            return StatusCode((int)res.Code, res);
        }


    }
}
