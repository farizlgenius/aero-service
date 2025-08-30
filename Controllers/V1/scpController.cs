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
using HIDAeroService.Service.Interface;
using HIDAeroService.Service.Impl;

namespace HIDAeroService.Controllers.V1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class scpController : ControllerBase
    {
        private readonly ScpService _scpService;
        private readonly SioService _sioService;
        public scpController(ScpService scpService, SioService sioService)
        {
            _scpService = scpService;
            _sioService = sioService;

        }

        [HttpPost("status")]
        public ActionResult<BaseDto<ScpStatusDto>> GetScpStatus([FromBody] GetScpStatusDto dto)
        {
            BaseDto<ScpStatusDto> res = new BaseDto<ScpStatusDto>();
            res.StatusDesc = Constants.ConstantsHelper.SUCCESS;
            res.StatusCode = Constants.ConstantsHelper.SUCCESS_CODE;
            res.Time = DateTime.UtcNow;
            res.Content = _scpService.GetOnlineStatus(dto.ScpMac);
            return StatusCode(res.StatusCode, res);
        }

        [HttpGet("report/{id}")]
        public ActionResult<BaseDto<IDReportDto>> GetIDReportById(short id)
        {
            BaseDto<IDReportDto> dto = new BaseDto<IDReportDto>();
            dto.Content = _scpService.GetIDReport(id);
            if (dto.Content != null)
            {
                dto.StatusCode = Constants.ConstantsHelper.SUCCESS_CODE;
                dto.StatusDesc = Constants.ConstantsHelper.SUCCESS;
                dto.Time = DateTime.UtcNow;
                return StatusCode(dto.StatusCode, dto);
            }
            dto.StatusCode = Constants.ConstantsHelper.NOT_FOUND_CODE;
            dto.StatusDesc = Constants.ConstantsHelper.NOT_FOUND;
            dto.Time = DateTime.UtcNow;
            return StatusCode(dto.StatusCode, dto);
        }

        [HttpGet("report/all")]
        public async Task<ActionResult<BaseDto<List<IDReportDto>>>> GetIDReportAll()
        {
            BaseDto<List<IDReportDto>> dto = new BaseDto<List<IDReportDto>>();
            if (_scpService.IDReportListCount() != 0)
            {
                List<IDReportDto> data = await _scpService.IDReportList();
                dto.StatusCode = Constants.ConstantsHelper.SUCCESS_CODE;
                dto.StatusDesc = Constants.ConstantsHelper.SUCCESS;
                dto.Time = DateTime.Now.ToLocalTime();
                dto.Content = data;
                return Ok(dto);
            }
            dto.StatusCode = Constants.ConstantsHelper.NOT_FOUND_CODE;
            dto.StatusDesc = Constants.ConstantsHelper.NOT_FOUND;
            dto.Time = DateTime.Now.ToLocalTime();
            dto.Content = new List<IDReportDto>();
            return Ok(dto);
        }


        [HttpGet("all")]
        public ActionResult<BaseDto<List<ScpDto>>> GetScpList()
        {
            BaseDto<List<ScpDto>> dto = new BaseDto<List<ScpDto>>();
            var data = _scpService.GetScpList();
            if (data.Count != 0)
            {
                dto.StatusCode = Constants.ConstantsHelper.SUCCESS_CODE;
                dto.StatusDesc = Constants.ConstantsHelper.SUCCESS;
                dto.Time = DateTime.UtcNow;
                dto.Content = data;
                return Ok(dto);
            }
            dto.StatusCode = 404;
            dto.StatusDesc = Constants.ConstantsHelper.NOT_FOUND;
            dto.Time = DateTime.UtcNow;
            dto.Content = new List<ScpDto>();
            return Ok(dto);
        }

        [HttpPost("create")]
        public ActionResult<BaseDto> Create([FromBody] ScpRegisDto scpDto)
        {
            BaseDto res = new BaseDto();
            //res.StatusDesc = _scpService.ScpConfiguration(scpDto.ScpId, _tzService, _cardFormatService, _alvlService);
            //if (res.StatusDesc != AppContants.COMMAND_SUCCESS)
            //{
            //    res.Time = DateTime.UtcNow;
            //    res.StatusCode = AppContants.INTERNAL_ERROR_CODE;
            //    return StatusCode(res.StatusCode, res);
            //}

            if (!_scpService.RegisterSCP(scpDto))
            {
                res.StatusDesc = "Internal Error";
                res.Time = DateTime.UtcNow;
                res.StatusCode = Constants.ConstantsHelper.INTERNAL_ERROR_CODE;
                return StatusCode(res.StatusCode, res);

            }

            short SioNo = 0;
            short model = 196;
            short address = 0;
            short msp1_number = 0;
            short internal_port_number = 3;
            short internal_baud_rate = -1;
            short protocol = 0;
            short n_dialect = 0;


            if (!_sioService.Save(scpDto.Mac, scpDto.Name, scpDto.Name, scpDto.Ip, SioNo, model, address, msp1_number, internal_port_number, internal_baud_rate, protocol, n_dialect))
            {
                res.StatusDesc = "Internal Error";
                res.Time = DateTime.UtcNow;
                res.StatusCode = Constants.ConstantsHelper.INTERNAL_ERROR_CODE;
                return StatusCode(res.StatusCode, res);
            }


            if (!_scpService.ReadStructureStatus(scpDto.ScpId))
            {
                res.StatusDesc = "Internal Error";
                res.Time = DateTime.UtcNow;
                res.StatusCode = Constants.ConstantsHelper.INTERNAL_ERROR_CODE;
                return StatusCode(res.StatusCode, res);
            }

            res.StatusCode = Constants.ConstantsHelper.CREATED_CODE;
            res.Time = DateTime.UtcNow;
            res.StatusDesc = Constants.ConstantsHelper.CREATED;
            return StatusCode(res.StatusCode, res);

        }

        [HttpPost("config/verify")]
        public ActionResult<BaseDto> VerifyScpConfiguration(VerifyScpDto dto)
        {
            BaseDto res = new BaseDto();
            res.StatusDesc = _scpService.VerifyScpConfiguration(dto);
            if (res.StatusDesc == Constants.ConstantsHelper.COMMAND_SUCCESS)
            {
                res.StatusDesc = Constants.ConstantsHelper.SUCCESS;
                res.StatusCode = 200;
                res.Time = DateTime.UtcNow;
                return base.StatusCode(200, res);
            }
            res.StatusDesc = Constants.ConstantsHelper.COMMAND_UNSUCCESS;
            res.StatusCode = 500;
            res.Time = DateTime.UtcNow;
            return StatusCode(res.StatusCode, res);
        }


        [HttpPost("reset")]
        public ActionResult<BaseDto> ResetScp([FromBody] ResetScpDto dto)
        {
            BaseDto res = new BaseDto();
            if (!_scpService.ResetScp(dto))
            {
                res.StatusDesc = "Internal Error";
                res.StatusCode = 500;
                res.Time = DateTime.UtcNow;
                return StatusCode(res.StatusCode, res); ;
            }

            res.StatusDesc = "OK";
            res.StatusCode = 200;
            res.Time = DateTime.UtcNow;
            return StatusCode(res.StatusCode, res); ;
        }


        [HttpPost("read/{id}")]
        public ActionResult<BaseDto> ReadMemory(short id)
        {
            BaseDto res = new BaseDto();
            if (!_scpService.ReadStructureStatus(id))
            {
                res.StatusDesc = "Internal Error";
                res.StatusCode = 500;
                res.Time = DateTime.UtcNow;
                return res;
            }

            res.StatusDesc = Constants.ConstantsHelper.SUCCESS;
            res.StatusCode = Constants.ConstantsHelper.SUCCESS_CODE;
            res.Time = DateTime.UtcNow;
            return res;
        }

        [HttpGet("webconfig/{id}")]
        public ActionResult<BaseDto> GetWebConfig(short id)
        {
            BaseDto res = new BaseDto();
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
        public ActionResult<BaseDto> RemoveScp([FromBody] RemoveScpDto dto)
        {
            BaseDto res = new BaseDto();
            res.StatusDesc = _scpService.RemoveScp(dto);
            if (res.StatusDesc == Constants.ConstantsHelper.REMOVE_SUCCESS)
            {
                res.StatusCode = Constants.ConstantsHelper.SUCCESS_CODE;
                res.Time = DateTime.UtcNow;
                return base.StatusCode(res.StatusCode, res);
            }
            res.StatusCode = Constants.ConstantsHelper.INTERNAL_ERROR_CODE;
            res.Time = DateTime.UtcNow;
            return StatusCode(res.StatusCode, res);
        }

        [HttpPost("config/upload")]
        public ActionResult<BaseDto> UploadScpConfig(UploadScpConfigDto dto)
        {
            BaseDto res = new BaseDto();
            res.StatusDesc = _scpService.UploadScpConfig(dto);
            if (res.StatusDesc == Constants.ConstantsHelper.COMMAND_SUCCESS)
            {
                res.StatusDesc = Constants.ConstantsHelper.SUCCESS;
                res.StatusCode = 200;
                res.Time = DateTime.UtcNow;
                return base.StatusCode(res.StatusCode, res);
            }
            res.StatusDesc = Constants.ConstantsHelper.INTERNAL_ERROR;
            res.StatusCode = Constants.ConstantsHelper.INTERNAL_ERROR_CODE;
            res.Time = DateTime.UtcNow;
            return StatusCode(res.StatusCode, res);
        }


    }
}
