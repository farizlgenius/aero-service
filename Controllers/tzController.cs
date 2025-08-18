using HIDAeroService.Dto;
using HIDAeroService.Dto.Time;
using HIDAeroService.Entity;
using HIDAeroService.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HIDAeroService.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public sealed class tzController : ControllerBase
    {
        private readonly TZService _tzService;

        public tzController(TZService tzService) 
        {
            _tzService = tzService;
        }

        [HttpGet("all")]
        public ActionResult<GenericDto<List<TZDto>>> GetTimeZoneList()
        {
           GenericDto<List<TZDto>> res = new GenericDto<List<TZDto>>();
            res.StatusDesc = "Ok";
            res.StatusCode = 200;
            res.Time = DateTime.UtcNow;
            res.Content = _tzService.GetTimeZoneDtoList();
            return res;
        }

        [HttpPost("add")]
        public ActionResult<GenericDto> Create(CreateTimeZoneDto dto)
        {
            GenericDto res = new GenericDto();
            if (!_tzService.CreateTimeZone(dto))
            {
                res.StatusDesc = "Create Time Zone Unsuccess";
                res.StatusCode = 500;
                res.Time = DateTime.UtcNow;
                return Ok(res);
            }
            res.StatusDesc = "Ok";
            res.StatusCode = 200;
            res.Time = DateTime.UtcNow;
            return Ok(res);
        }
    }
}
