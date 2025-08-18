using HIDAeroService.Dto;
using HIDAeroService.Mapper;
using HIDAeroService.Models;
using HIDAeroService.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HIDAeroService.Service;
using System.Collections.Generic;
using System.Buffers.Text;
using HIDAeroService.Entity;
using System.Net;
using HIDAeroService.Dto.Scp;
using HIDAeroService.Constants;

namespace HIDAeroService.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class scpController : ControllerBase
    {
        private readonly ScpService _scpService;
        private readonly SioService _sioService;
        private readonly CpService _cpService;
        private readonly MpService _mpService;
        public scpController(ScpService scpService,SioService sioService, CpService cpService, MpService mpService)
        {
            _scpService = scpService;
            _sioService = sioService;
            _cpService = cpService;
            _mpService = mpService;
        }

        [HttpPost("status")]
        public ActionResult<GenericDto<ScpStatusDto>> GetScpStatus([FromBody] GetScpStatusDto dto)
        {
            GenericDto < ScpStatusDto > res = new GenericDto<ScpStatusDto> ();
            res.StatusDesc = AppContants.SUCCESS;
            res.StatusCode = 200;
            res.Time = DateTime.UtcNow;
            res.Content = _scpService.GetOnlineStatus(dto.ScpId); ;
            return StatusCode(res.StatusCode,res);
        }

        [HttpGet("report/{id}")]
        public ActionResult<GenericDto<IDReportDto>> GetIDReportById(short id)
        {
            GenericDto<IDReportDto> dto = new GenericDto<IDReportDto>();
            dto.Content = _scpService.GetIDReport(id);
            if (dto.Content != null)
            {
                dto.StatusCode = 200;
                dto.StatusDesc = AppContants.SUCCESS;
                dto.Time = DateTime.UtcNow;
                return StatusCode(dto.StatusCode,dto);
            }
            dto.StatusCode = 404;
            dto.StatusDesc = AppContants.NOT_FOUND;
            dto.Time = DateTime.UtcNow;
            return StatusCode(dto.StatusCode,dto);
        }

        [HttpGet("report/all")]
        public async Task<ActionResult<GenericDto<List<IDReportDto>>>> GetIDReportAll() 
        {
            GenericDto<List<IDReportDto>> dto = new GenericDto<List<IDReportDto>>();
            if (_scpService.IDReportListCount() != 0)
            {
                List<IDReportDto> data = await _scpService.IDReportList(); 
                dto.StatusCode = 200;
                dto.StatusDesc = AppContants.SUCCESS;
                dto.Time = DateTime.Now.ToLocalTime();
                dto.Content = data;
                return StatusCode(dto.StatusCode,dto);
            }
            dto.StatusCode = 404;
            dto.StatusDesc = AppContants.NOT_FOUND;
            dto.Time = DateTime.Now.ToLocalTime();
            dto.Content = new List<IDReportDto>();
            return StatusCode(dto.StatusCode,dto);
        }

        [HttpGet("all")]
        public ActionResult<GenericDto<List<ScpDto>>> GetScpList() 
        {
            GenericDto<List<ScpDto>> dto = new GenericDto<List<ScpDto>>();
            var data = _scpService.GetScpList();
            if(data.Count != 0)
            {
                dto.StatusCode = 200;
                dto.StatusDesc = AppContants.SUCCESS;
                dto.Time = DateTime.UtcNow;
                dto.Content = data;
                return StatusCode(dto.StatusCode,dto);
            }
            dto.StatusCode = 404;
            dto.StatusDesc = AppContants.NOT_FOUND;
            dto.Time = DateTime.UtcNow;
            return StatusCode(dto.StatusCode,dto);
        }

        [HttpPost("create")]
        public ActionResult<GenericDto> Create([FromForm]ScpRegisDto scpDto)
        {
            GenericDto res = new GenericDto();
            if (!_scpService.InitialScpConfiguration(scpDto.ScpId, _cpService, _mpService, scpDto.Mac))
            {
                res.StatusDesc = "Internal Error";
                res.Time = DateTime.Now.ToLocalTime();
                res.StatusCode = 500;
                return StatusCode(500, res);
            }

            if (!_scpService.RegisterSCP(scpDto))
            {
                res.StatusDesc = "Internal Error";
                res.Time = DateTime.Now.ToLocalTime();
                res.StatusCode = 500;
                return StatusCode(500, res);

            }

            short SioNo = 0;
            short model = 196;
            short address = 0;
            short msp1_number = 0;
            short internal_port_number = 3;
            short internal_baud_rate = -1;
            short protocol = 0;
            short n_dialect = 0;


            if (!_sioService.SaveToDataBase(scpDto.Name,scpDto.Name,scpDto.Ip, SioNo, model, address, msp1_number, internal_port_number, internal_baud_rate, protocol, n_dialect))
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

        [HttpPost("verify/config")]
        public ActionResult<GenericDto> VerifyScpConfiguration(VerifyScpDto dto)
        {
            GenericDto res = new GenericDto();
            res.StatusDesc = _scpService.VerifyScpConfiguration(dto);
            if(res.StatusDesc == AppContants.COMMAND_SUCCESS)
            {
                res.StatusDesc = AppContants.SUCCESS;
                res.StatusCode = 200;
                res.Time = DateTime.UtcNow;
                return StatusCode(200, res);
            }
            res.StatusDesc = AppContants.COMMAND_UNSUCCESS;
            res.StatusCode = 500;
            res.Time = DateTime.UtcNow;
            return StatusCode(res.StatusCode,res);
        }

        [HttpPost("reset")]
        public ActionResult<GenericDto> RestSCP([FromBody] ResetScpDto dto)
        {
            GenericDto res = new GenericDto();
            if (!_scpService.ResetScp(dto))
            {
                res.StatusDesc = "Internal Error";
                res.StatusCode = 500;
                res.Time = DateTime.UtcNow;
                return res;
            }

            res.StatusDesc = "OK";
            res.StatusCode = 200;
            res.Time = DateTime.UtcNow;
            return res;
        }

        
        [HttpPost("read/{id}")]
        public ActionResult<GenericDto> ReadMemory(short id)
        {
            GenericDto res = new GenericDto();
            if (!_scpService.ReadStructureStatus(id))
            {
                res.StatusDesc = "Internal Error";
                res.StatusCode = 500;
                res.Time = DateTime.UtcNow;
                return res;
            }

            res.StatusDesc = "OK";
            res.StatusCode = 200;
            res.Time = DateTime.UtcNow;
            return res;
        }

        [HttpGet("webconfig/{id}")]
        public ActionResult<GenericDto> GetWebConfig(short id)
        {
            GenericDto res = new GenericDto();
            if (!_scpService.GetWenConfig(id))
            {
                res.StatusDesc = "Internal Error";
                res.StatusCode = 500;
                res.Time = DateTime.UtcNow;
                return res;
            }

            res.StatusDesc = "OK";
            res.StatusCode = 200;
            res.Time = DateTime.UtcNow;
            return res;
        }

        [HttpPost("remove")]
        public ActionResult<GenericDto> RemoveScp(RemoveScpDto dto)
        {
            GenericDto res = new GenericDto();
            res.StatusDesc = _scpService.RemoveScp(dto);
            if (res.StatusDesc == AppContants.REMOVE_SUCCESS)
            {
                res.StatusCode = 200;
                res.Time = DateTime.UtcNow;
                return StatusCode(res.StatusCode, res);
            }
            res.StatusCode = 500;
            res.Time = DateTime.UtcNow;
            return StatusCode(res.StatusCode,res);
        }

        [HttpPost("upload")]
        public ActionResult<GenericDto> UploadScpConfig(UploadScpConfigDto dto)
        {
            GenericDto res = new GenericDto();
            res.StatusDesc = _scpService.UploadScpConfig(dto);
            if(res.StatusDesc == AppContants.COMMAND_SUCCESS)
            {
                res.StatusDesc = AppContants.SUCCESS;
                res.StatusCode = 200;
                res.Time = DateTime.UtcNow;
                return StatusCode(res.StatusCode, res);
            }
            res.StatusDesc = AppContants.COMMAND_UNSUCCESS;
            res.StatusCode = 500;
            res.Time = DateTime.UtcNow;
            return StatusCode(res.StatusCode,res);
        }


    }
}
