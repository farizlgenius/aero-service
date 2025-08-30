using HIDAeroService.Constants;
using HIDAeroService.Data;
using HIDAeroService.Dto.Holiday;
using HIDAeroService.Helpers;
using HIDAeroService.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace HIDAeroService.Controllers.V1
{
    [Route("api/[controller]")]
    [ApiController]
    public class holidayController : ControllerBase
    {
        private readonly IHolidayService _holidayService;
        private readonly AppDbContext _context;
        public holidayController(AppDbContext context,IHolidayService holidayService) 
        {
            _context = context;
            _holidayService = holidayService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<HolidayDto>>> GetAll()
        {
            var res = await _holidayService.GetAsync();
            return StatusCode((int)res.Code, res);
        }

        [HttpPost]
        public async Task<ActionResult<HolidayDto>> CreateAsync([FromBody] HolidayDto dto)
        {
            var res = await _holidayService.CreateAsync(dto);
            return StatusCode((int)res.Code, res);
        }

        [HttpPut]
        public async Task<ActionResult<HolidayDto>> Update([FromBody] HolidayDto dto)
        {
            var res = await _holidayService.UpdateAsync(dto);
            return StatusCode((int)res.Code, res);
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult<HolidayDto>> RemoveAsync(short id)
        {
            var res = await _holidayService.DeleteAsync(id);
            return StatusCode((int)res.Code, res);
        }

    }
}
