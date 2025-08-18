using HIDAeroService.Dto;
using HIDAeroService.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HIDAeroService.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class mpController : ControllerBase
    {
        private readonly MpService _mpService;
        public mpController(MpService mpService)
        {
            _mpService = mpService;
        }

        [HttpGet("all")]
        public ActionResult<GenericDto<List<MpDto>>> GetMonitorPointList()
        {
            GenericDto<List<MpDto>> res = new GenericDto<List<MpDto>>();
            List<MpDto> content = _mpService.GetMonitorPointList();
            if (content.Count > 0)
            {
                res.Content = content;
                res.StatusDesc = "OK";
                res.StatusCode = 200;
                res.Time = DateTime.UtcNow;
                return Ok(res);
            }
            res.StatusCode = 200;
            res.Time = DateTime.UtcNow;
            res.StatusDesc = "Record empty";
            res.Content = [];
            return Ok(res);
        }

        [HttpPost("add")]
        public ActionResult<GenericDto> Create([FromForm] AddMpDto dto)
        {
            GenericDto res = new GenericDto();
            if (!_mpService.CreateMonitorPoint(dto.Name,dto.ScpIp,dto.SioNumber,dto.IpNumber,dto.LcvtMode,dto.Debounce,dto.HoldTime,dto.LfCode,dto.Mode,dto.DelayEntry,dto.DelayExit,_mpService))
            {
                res.StatusDesc = "Internal Error";
                res.Time = DateTime.Now.ToLocalTime();
                res.StatusCode = 500;
                return StatusCode(500, res);

            }

            res.StatusCode = 201;
            res.Time = DateTime.Now.ToLocalTime();
            res.StatusDesc = "Created";
            return StatusCode(201, res);
        }

        [HttpGet("{sio}")]
        public ActionResult<GenericDto<List<short>>> GetAvailableIp(short sio)
        {
            GenericDto<List<short>> res = new GenericDto<List<short>>();
            List<short> dtos = _mpService.GetAvailableIp(sio);
            if (dtos.Count > 0)
            {
                res.Content = dtos;
                res.StatusDesc = "OK";
                res.StatusCode = 200;
                res.Time = DateTime.UtcNow;
                return Ok(res);
            }
            res.StatusCode = 500;
            res.Time = DateTime.UtcNow;
            res.StatusDesc = "Can't get list";
            return StatusCode(500, res);
        }

        [HttpGet("status")]
        public ActionResult<GenericDto> GetMpStatus([FromQuery] string ScpIp, short MpNo)
        {
            GenericDto res = new GenericDto();
            if (!_mpService.GetMpStatus(ScpIp, MpNo))
            {
                res.StatusCode = 500;
                res.Time = DateTime.UtcNow;
                res.StatusDesc = "Can't get status";
                return StatusCode(500, res);
            }

            res.StatusDesc = "OK";
            res.StatusCode = 200;
            res.Time = DateTime.UtcNow;
            return Ok(res);
        }

        [HttpPost("remove")]
        public ActionResult<GenericDto> RemoveControlPoint([FromQuery] string ScpIp, short MpNo)
        {
            GenericDto res = new GenericDto();
            if (!_mpService.RemoveMonitorPoint(ScpIp, MpNo))
            {
                res.StatusCode = 500;
                res.Time = DateTime.UtcNow;
                res.StatusDesc = "Can't get status";
                return StatusCode(500, res);
            }

            res.StatusDesc = "OK";
            res.StatusCode = 200;
            res.Time = DateTime.UtcNow;
            return Ok(res);
        }

        [HttpGet("mode")]
        public ActionResult<GenericDto<List<IpModeDto>>> GetInputModeList()
        {
            GenericDto<List<IpModeDto>> res = new GenericDto<List<IpModeDto>>();
            List<IpModeDto> dtos = _mpService.GetIpModeList();
            if (dtos.Count > 0)
            {
                res.Content = dtos;
                res.StatusDesc = "OK";
                res.StatusCode = 200;
                res.Time = DateTime.UtcNow;
                return Ok(res);
            }
            res.StatusCode = 500;
            res.Time = DateTime.UtcNow;
            res.StatusDesc = "Can't get mode";
            return StatusCode(500, res);
        }
    }
}
