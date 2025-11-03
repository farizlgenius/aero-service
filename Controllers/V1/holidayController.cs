using HIDAeroService.DTO;
using HIDAeroService.DTO.Holiday;
using HIDAeroService.Entity;
using HIDAeroService.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.Net;

namespace HIDAeroService.Controllers.V1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class HolidayController(IHolidayService holService) : ControllerBase
    {


        [HttpGet]
        public async Task<ActionResult<ResponseDto<IEnumerable<HolidayDto>>>> GetAsync()
        {
            var res = await holService.GetAsync();
            return Ok(res);
        }

        [HttpGet("{component}")]
        public async Task<ActionResult<ResponseDto<HolidayDto>>> GetByComponentAsync(short component)
        {
            var res = await holService.GetByComponentIdAsync(component);
            return  Ok(res);
        }

        [HttpPost("/clear")]
        public async Task<ActionResult<ResponseDto<HolidayDto>>> ClearAsync()
        {
            var res = await holService.ClearAsync();
            return Ok(res);
        }

        [HttpPost]
        public async Task<ActionResult<ResponseDto<HolidayDto>>> CreateAsync([FromBody] CreateHolidayDto dto)
        {
            var res = await holService.CreateAsync(dto);
            return Ok(res);
        }

        [HttpPut]
        public async Task<ActionResult<ResponseDto<HolidayDto>>> UpdateAsync([FromBody] HolidayDto dto)
        {
            var res = await holService.UpdateAsync(dto);
            return Ok(res);
        }

        [HttpDelete("{component}")]
        public async Task<ActionResult<ResponseDto<HolidayDto>>> DeleteByIdAsync(short component)
        {
            var res = await holService.DeleteAsync(component);
            return Ok(res);
        }


    }
}
