using Aero.Application.DTOs;
using Aero.Application.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aero.Api.Controllers.V1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class HolidayController(IHolidayService service) : ControllerBase
    {


        [HttpGet]
        [Authorize]
        public async Task<ActionResult<ResponseDto<IEnumerable<HolidayDto>>>> GetAsync()
        {
            var res = await service.GetAsync();
            return Ok(res);
        }

        [HttpGet("{component}")]
        [Authorize]
        public async Task<ActionResult<ResponseDto<HolidayDto>>> GetByComponentAsync(short component)
        {
            var res = await service.GetByComponentIdAsync(component);
            return  Ok(res);
        }

        [HttpGet("/api/v1/{location}/[controller]")]
        [Authorize]
        public async Task<ActionResult<ResponseDto<IEnumerable<HolidayDto>>>> GetByLocationAsync(short location)
        {
            var res = await service.GetByLocationAsync(location);
            return Ok(res);
        }

        [HttpPost("/clear")]
        [Authorize]
        public async Task<ActionResult<ResponseDto<HolidayDto>>> ClearAsync()
        {
            var res = await service.ClearAsync();
            return Ok(res);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<ResponseDto<HolidayDto>>> CreateAsync([FromBody] HolidayDto dto)
        {
            var res = await service.CreateAsync(dto);
            return Ok(res);
        }

        [HttpPut]
        [Authorize]
        public async Task<ActionResult<ResponseDto<HolidayDto>>> UpdateAsync([FromBody] HolidayDto dto)
        {
            var res = await service.UpdateAsync(dto);
            return Ok(res);
        }

        [HttpDelete("{component}")]
        [Authorize]
        public async Task<ActionResult<ResponseDto<HolidayDto>>> DeleteByIdAsync(short component)
        {
            var res = await service.DeleteAsync(component);
            return Ok(res);
        }

        [HttpPost("delete/range")]
        [Authorize]
        public async Task<ActionResult<ResponseDto<IEnumerable<ResponseDto<bool>>>>> DeleteRangeAsync([FromBody] List<short> components)
        {
            var res = await service.DeleteRangeAsync(components);
            return Ok(res);
        }


    }
}
