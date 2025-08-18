using HIDAeroService.Models;
using HIDAeroService.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HIDAeroService.Dto;
using HID.Aero.ScpdNet.Wrapper;
using HIDAeroService.Service;

namespace HIDAeroService.Controllers
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
        public ActionResult<GenericDto<List<CpDto>>> GetControlPointList()
        {
            GenericDto<List<CpDto>> res = new GenericDto<List<CpDto>>();
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
        public ActionResult<GenericDto> Create([FromForm] AddCpDto dto)
        {
            GenericDto res = new GenericDto();
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
        public ActionResult<GenericDto> TriggerContactPoint([FromQuery] CpTriggerDto cpTriggerDto)
        {
            GenericDto res = new GenericDto();
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
        public ActionResult<GenericDto<List<OpModeDto>>> GetOpModeList()
        {
            GenericDto<List<OpModeDto>> res = new GenericDto<List<OpModeDto>>();
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
        public ActionResult<GenericDto<List<short>>> GetAvailableOp(short sio)
        {
            GenericDto<List<short>> res = new GenericDto<List<short>>();
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

        [HttpGet("status")]
        public ActionResult<GenericDto> GetCpStatus([FromQuery]string ScpIp,short CpNo)
        {
            GenericDto res = new GenericDto();
            if (!_cpService.GetCpStatus(ScpIp,CpNo))
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
        public ActionResult<GenericDto> RemoveControlPoint([FromQuery]string ScpIp,short CpNo)
        {
            GenericDto res = new GenericDto();
            if (!_cpService.RemoveControlPoint(ScpIp, CpNo))
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
