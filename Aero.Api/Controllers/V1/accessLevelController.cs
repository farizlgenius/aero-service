using AeroService.Entity;
using AeroService.Service.Impl;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using AeroService.Constants;
using AeroService.Service;
using AeroService.DTO;
using AeroService.DTO.AccessLevel;


namespace AeroService.Controllers.V1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public sealed class AccessLevelController(IAccessLevelService accesslevelService) : ControllerBase
    {

        [HttpGet]
        public async Task<ActionResult<ResponseDto<IEnumerable<AccessLevelDto>>>> GetAsync()
        {
            var res = await accesslevelService.GetAsync();
            return Ok(res);
        }

        [HttpGet("/api/v1/{location}/[controller]")]
        public async Task<ActionResult<ResponseDto<IEnumerable<AccessLevelDto>>>> GetByLocationIdAsync(short location)
        {
            var res = await accesslevelService.GetByLocationIdAsync(location);
            return Ok(res);
        }

        [HttpGet("{component}")]
        public async Task<ActionResult<ResponseDto<AccessLevelDto>>> GetByComponentAsync(short component)
        {
            var  res = await accesslevelService.GetByComponentIdAsync(component);
            return Ok(res);
        }

        [HttpPost]
        public async Task<ActionResult<ResponseDto<AccessLevelDto>>> CreateAsync([FromBody] CreateUpdateAccessLevelDto dto)
        {
            var res = await accesslevelService.CreateAsync(dto);
            return Ok(res);
        }

        [HttpDelete("{component}")]
        public async Task<ActionResult<ResponseDto<AccessLevelDto>>> DeleteAsync(short component)
        {
            var res = await accesslevelService.DeleteAsync(component);
            return Ok(res);
        }

        [HttpPut]
        public async Task<ActionResult<ResponseDto<AccessLevelDto>>> UpdateAsync([FromBody] CreateUpdateAccessLevelDto dto)
        {
            var res = await accesslevelService.UpdateAsync(dto);
            return Ok(res);
        }
    }
}
