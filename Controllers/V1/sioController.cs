using HIDAeroService.Dto;
using HIDAeroService.Dto.Sio;
using HIDAeroService.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace HIDAeroService.Controllers.V1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class sioController : ControllerBase
    {
        private readonly SioService _sioService;

        public sioController(SioService sioService)
        {
            _sioService = sioService;
        }

        [HttpGet("status")]
        public IActionResult GetSioStatus([FromQuery] string ScpIp, short SioNo)
        {
            _sioService.GetSioStatus(ScpIp, SioNo);
            return Ok(new { Status = "OK" });
        }


        [HttpGet("all")]
        public ActionResult<BaseDto<List<SioDto>>> GetSioList()
        {
            BaseDto<List<SioDto>> res = new BaseDto<List<SioDto>>();
            List<SioDto> dtos = _sioService.GetSioList();
            if (dtos.Count != 0)
            {
                res.StatusDesc = "OK";
                res.Content = dtos;
                res.StatusCode = 200;
                res.Time = DateTime.UtcNow;
                return res;
            }
            res.Content = new List<SioDto>();
            res.StatusDesc = "Record is empty";
            res.StatusCode = 404;
            res.Time = DateTime.UtcNow;
            return res;

        }

        [HttpGet("{mac}")]
        public ActionResult<BaseDto<List<SioDto>>> GetSioListFromScpIp(string mac)
        {
            BaseDto<List<SioDto>> res = new BaseDto<List<SioDto>>();
            List<SioDto> dtos = _sioService.GetSioListFromMac(mac);
            if (dtos.Count > 0)
            {
                res.StatusDesc = "OK";
                res.Content = dtos;
                res.StatusCode = 200;
                res.Time = DateTime.UtcNow;
                return res;
            }
            res.StatusDesc = "Record is empty";
            res.StatusCode = 200;
            res.Time = DateTime.UtcNow;
            return res;
        }
    }
}
