using HIDAeroService.Constants;
using HIDAeroService.Dto;
using HIDAeroService.Dto.Mp;
using HIDAeroService.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HIDAeroService.Controllers.V1
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
        public ActionResult<BaseDto<List<MpDto>>> GetMonitorPointList()
        {
            BaseDto<List<MpDto>> res = new BaseDto<List<MpDto>>();
            List<MpDto> content = _mpService.GetMonitorPointList();
            if (content.Count > 0)
            {
                res.Content = content;
                res.StatusDesc = Constants.ConstantsHelper.SUCCESS;
                res.StatusCode = Constants.ConstantsHelper.SUCCESS_CODE;
                res.Time = DateTime.UtcNow;
                return Ok(res);
            }
            res.StatusCode = Constants.ConstantsHelper.SUCCESS_CODE;
            res.Time = DateTime.UtcNow;
            res.StatusDesc = Constants.ConstantsHelper.NOT_FOUND_RECORD;
            res.Content = [];
            return Ok(res);
        }

        [HttpPost("add")]
        public ActionResult<BaseDto> Create([FromBody] AddMpDto dto)
        {
            BaseDto res = new BaseDto();
            if (!_mpService.CreateMonitorPoint(dto))
            {
                res.StatusDesc = Constants.ConstantsHelper.INTERNAL_ERROR;
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
        public ActionResult<BaseDto<List<short>>> GetAvailableIp(short sio)
        {
            BaseDto<List<short>> res = new BaseDto<List<short>>();
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

        [HttpPost("status")]
        public ActionResult<BaseDto> GetMpStatus([FromBody] GetMpStatusDto dto)
        {
            BaseDto res = new BaseDto();
            if (!_mpService.GetMpStatus(dto))
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
        public ActionResult<BaseDto> RemoveControlPoint([FromBody] RemoveMpDto dto)
        {
            BaseDto res = new BaseDto();
            if (!_mpService.RemoveMonitorPoint(dto))
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
        public ActionResult<BaseDto<List<IpModeDto>>> GetInputModeList()
        {
            BaseDto<List<IpModeDto>> res = new BaseDto<List<IpModeDto>>();
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
