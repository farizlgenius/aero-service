using Aero.Application.DTOs;
using Aero.Application.Interface;
using Aero.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aero.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController(ILocationService locationService) : ControllerBase
    {
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<ResponseDto<IEnumerable<LocationDto>>>> GetAsync()
        {
            var res = await locationService.GetAsync();
            return Ok(res);
        }

        [HttpGet("/api/{location}/[controller]")]
        [Authorize]
        public async Task<ActionResult<ResponseDto<IEnumerable<LocationDto>>>> GetPaginationAsync([FromQuery] PaginationParamsWithFilter param,short location)
        {
            var res = await locationService.GetPaginationAsync(param,location);
            return Ok(res); 
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<ResponseDto<bool>>> CreateAsync([FromBody] LocationDto dto)
        {
            var res = await locationService.CreateAsync(dto);
            return Ok(res);
        }

        [HttpPut]
        [Authorize]
        public async Task<ActionResult<ResponseDto<LocationDto>>> UpdateAsync([FromBody] LocationDto dto) 
        {
            var res = await locationService.UpdateAsync(dto);
            return Ok(res);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult<ResponseDto<bool>>> DeleteByIdAsync(int id)
        {
            var res = await locationService.DeleteByIdAsync(id);
            return Ok(res);
        }

        [HttpPost("delete/range")]
        public async Task<ActionResult<ResponseDto<IEnumerable<ResponseDto<bool>>>>> DeleteRangeAsync([FromBody] List<int> ids)
        {
            var res = await locationService.DeleteRangeAsync(ids);
            return Ok(res);
        }

        [HttpPost("range")]
        [Authorize]
        public async Task<ActionResult<ResponseDto<IEnumerable<LocationDto>>>> GetRangeLocationById([FromBody] LocationRangeDto locationIds)
        {
            var res = await locationService.GetRangeLocationById(locationIds);
            return Ok(res);
        }
        
    }
}
