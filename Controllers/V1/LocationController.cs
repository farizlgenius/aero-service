using HIDAeroService.DTO;
using HIDAeroService.DTO.Location;
using HIDAeroService.Entity.Interface;
using HIDAeroService.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SixLabors.ImageSharp;

namespace HIDAeroService.Controllers.V1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class LocationController(ILocationService locationService) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<ResponseDto<IEnumerable<LocationDto>>>> GetAsync()
        {
            var res = await locationService.GetAsync();
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
        public async Task<ActionResult<ResponseDto<LocationDto>>> UpdateAsync([FromBody] LocationDto dto) 
        {
            var res = await locationService.UpdateAsync(dto);
            return Ok(res);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ResponseDto<bool>>> DeleteByIdAsync(short id)
        {
            var res = await locationService.DeleteByIdAsync(id);
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
