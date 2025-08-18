using HIDAeroService.Dto;
using HIDAeroService.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace HIDAeroService.Controllers
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
        public IActionResult GetSioStatus([FromQuery]string ScpIp,short SioNo) 
        {
            _sioService.GetSioStatus(ScpIp, SioNo);
            return Ok(new { Status = "OK" });
        }


        [HttpGet("all")]
        public ActionResult<GenericDto<List<SioDto>>> GetSioList()
        {
            GenericDto < List < SioDto >> res  = new GenericDto<List<SioDto>> ();
            List<SioDto> dtos = _sioService.GetSioList();
            if(dtos.Count != 0)
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

        [HttpGet("{ip}")]
        public ActionResult<GenericDto<List<SioDto>>> GetSioListFromScpIp(string ip)
        {
            GenericDto<List<SioDto>> res = new GenericDto<List<SioDto>> ();
            List<SioDto> dtos = _sioService.GetSioListFromIp(ip);
            if(dtos.Count > 0)
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
