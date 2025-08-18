using HIDAeroService.Dto;
using HIDAeroService.Dto.AccessLevel;
using HIDAeroService.Entity;
using HIDAeroService.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HIDAeroService.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public sealed class acslvController : ControllerBase
    {
        private readonly AlvlService _alvlService;
        public acslvController(AlvlService alvlService)
        {
            _alvlService = alvlService;
        }

        [HttpGet("all")]
        public ActionResult<GenericDto<List<AccessLevelDto>>> GetAccessLevelList()
        {
            GenericDto<List<AccessLevelDto>> res = new GenericDto<List<AccessLevelDto>>();
            List<AccessLevelDto> list = _alvlService.GetAccessLevelDtoList();
            if(list.Count > 0)
            {
                res.StatusCode = 200;
                res.StatusDesc = "Ok";
                res.Time = DateTime.UtcNow;
                res.Content = list;
                return StatusCode(res.StatusCode, res);
            }
            res.StatusDesc = "Empty";
            res.StatusCode = 404;
            res.Time = DateTime.UtcNow;
            res.Content = [];
            return StatusCode(res.StatusCode,res);
        }

        [HttpPost("tz/{id}")]
        public ActionResult<GenericDto<AccessLevelTimeZoneDto>> GetAccessLevelTimeZone(short id)
        {
            GenericDto<AccessLevelTimeZoneDto> res = new GenericDto<AccessLevelTimeZoneDto>();
            AccessLevelTimeZoneDto data = _alvlService.GetAccessLevelTimeZone(id);
            if (data != null)
            {
                res.StatusCode = 200;
                res.StatusDesc = "Ok";
                res.Time = DateTime.UtcNow;
                res.Content = data;
                return StatusCode(res.StatusCode, res);
            }
            res.StatusDesc = "Not Found";
            res.StatusCode = 404;
            res.Time = DateTime.UtcNow;
            return StatusCode(res.StatusCode, res);
        }

        [HttpPost("add")]
        public ActionResult<GenericDto> Create(CreateAccessLevelDto dto)
        {
            GenericDto res = new GenericDto();
            res.StatusDesc = _alvlService.CreateAccessLevel(dto);
            if(res.StatusDesc == "Created")
            {
                res.StatusCode = 201;
            }
            else
            {
                res.StatusCode = 500;
            }
            res.Time = DateTime.UtcNow;
            return StatusCode(res.StatusCode,res);
        }

        [HttpPost("delete")]
        public ActionResult<GenericDto> Delete(AccessLevelDto dto)
        {
            GenericDto res = new GenericDto();
            res.StatusDesc = _alvlService.RemoveAccessLevel(dto);
            if (res.StatusDesc == "Removed")
            {
                res.StatusCode = 200;
            }
            else
            {
                res.StatusCode = 500;
            }
            res.Time = DateTime.UtcNow;
            return StatusCode(res.StatusCode, res);
        } 

    }
}
