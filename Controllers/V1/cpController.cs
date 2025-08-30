using HIDAeroService.Models;
using HIDAeroService.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HIDAeroService.Dto;
using HID.Aero.ScpdNet.Wrapper;
using HIDAeroService.Service;
using HIDAeroService.Dto.Cp;
using HIDAeroService.Dto.Scp;

namespace HIDAeroService.Controllers.V1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class cpController : ControllerBase
    {
        private readonly CpService _cpService;
        public cpController(CpService cpService)
        {
            _cpService = cpService;
        }

        [HttpGet("all")]
        public ActionResult<BaseDto<List<CpDto>>> GetControlPointList()
        {
            BaseDto<List<CpDto>> res = new BaseDto<List<CpDto>>();
            List<CpDto> dtos = _cpService.GetControlPointList();
            if (dtos.Count > 0)
            {
                res.Content = dtos;
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
        public ActionResult<BaseDto> Create([FromBody] AddCpDto dto)
        {
            BaseDto res = new BaseDto();
            if (!_cpService.CreateControlPoint(dto))
            {
                res.StatusDesc = "Internal Error";
                res.Time = DateTime.UtcNow;
                res.StatusCode = 500;
                return StatusCode(500, res);

            }
            res.StatusDesc = "Created";
            res.Time = DateTime.UtcNow;
            res.StatusCode = 201;
            return StatusCode(201, res);
        }

        [HttpPost("trigger")]
        public ActionResult<BaseDto> TriggerContactPoint([FromBody] CpTriggerDto cpTriggerDto)
        {
            BaseDto res = new BaseDto();
            if (
                !_cpService.TriggerCP(cpTriggerDto))
            {
                res.StatusCode = 500;
                res.Time = DateTime.UtcNow;
                res.StatusDesc = "Trigger Cp Command Error";
                return StatusCode(500, res);
            }
            res.StatusCode = 200;
            res.Time = DateTime.UtcNow;
            res.StatusDesc = "OK";
            return Ok(res);
        }

        [HttpGet("mode")]
        public ActionResult<BaseDto<List<OpModeDto>>> GetOpModeList()
        {
            BaseDto<List<OpModeDto>> res = new BaseDto<List<OpModeDto>>();
            List<OpModeDto> dtos = _cpService.GetOpModeList();
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

        [HttpGet("{sio}")]
        public ActionResult<BaseDto<List<short>>> GetAvailableOp(short sio)
        {
            BaseDto<List<short>> res = new BaseDto<List<short>>();
            List<short> dtos = _cpService.GetAvailableOp(sio);
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
        public ActionResult<BaseDto> GetCpStatus([FromBody] GetCpStatusDto dto)
        {
            BaseDto res = new BaseDto();
            if (!_cpService.GetCpStatus(dto))
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
        public ActionResult<BaseDto> RemoveControlPoint([FromBody] RemoveCpDto dto)
        {
            BaseDto res = new BaseDto();
            if (!_cpService.RemoveControlPoint(dto))
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



    }
}
