
using HIDAeroService.DTO;
using HIDAeroService.DTO.Interval;
using HIDAeroService.Entity;
using HIDAeroService.Service;
using HIDAeroService.Service.Impl;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Net;

namespace HIDAeroService.Controllers.V1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public sealed class IntervalController(IIntervalService baseService) : ControllerBase
    {

        [HttpGet]
        public async Task<ActionResult<ResponseDto<IEnumerable<IntervalDto>>>> GetAsync()
        {
            var res = await baseService.GetAsync();
            return Ok(res);
        }

        [HttpPost]
        public async Task<ActionResult<ResponseDto<IntervalDto>>> CreateAsync([FromBody] CreateIntervalDto dto)
        {
            var res = await baseService.CreateAsync(dto);
            return Ok(res);
        }

        [HttpDelete("{component}")]
        public async Task<ActionResult<ResponseDto<IntervalDto>>> DeleteAsync(short component) 
        {
            var res = await baseService.DeleteAsync(component);
            return Ok(res);
        }

        [HttpPut]
        public async Task<ActionResult<ResponseDto<IntervalDto>>> UpdateAsync([FromBody] IntervalDto dto)
        {
            var res = await baseService.UpdateAsync(dto);
            return Ok(res);
        }

        [HttpGet("{component}")]
        public async Task<ActionResult<ResponseDto<IntervalDto>>> GetByComponentAsync(short component)
        {
            var res = await baseService.GetByIdAsync(component);
            return Ok(res);
        }
    }
}
