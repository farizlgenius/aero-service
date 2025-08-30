using HIDAeroService.Constants;
using HIDAeroService.Dto;
using HIDAeroService.Dto.Acr;
using HIDAeroService.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HIDAeroService.Controllers.V1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class acrController : ControllerBase
    {
        private readonly AcrService _acrService;

        public acrController(AcrService acrService)
        {
            _acrService = acrService;
        }

        [HttpGet("all")]
        public ActionResult<BaseDto<List<AcrDto>>> GetACRList()
        {
            BaseDto<List<AcrDto>> res = new BaseDto<List<AcrDto>>();
            List<AcrDto> dtos = _acrService.GetACRList();
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

        [HttpGet("{mac}")]
        public ActionResult<BaseDto<List<AcrDto>>> GetAcrListByMac(string mac)
        {
            BaseDto<List<AcrDto>> res = new BaseDto<List<AcrDto>>();
            List<AcrDto> dtos = _acrService.GetAcrListByMac(mac);
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
        public ActionResult<BaseDto> Create([FromBody] AddAcrDto dto)
        {
            BaseDto res = new BaseDto();
            if (!_acrService.CreateAccessControlReaderForCreate(dto))
            {
                res.StatusCode = Constants.ConstantsHelper.INTERNAL_ERROR_CODE;
                res.StatusDesc = Constants.ConstantsHelper.INTERNAL_ERROR;
                res.Time = DateTime.UtcNow;
                return StatusCode(res.StatusCode, res);
            }
            res.StatusCode = Constants.ConstantsHelper.SUCCESS_CODE;
            res.StatusDesc = Constants.ConstantsHelper.SUCCESS;
            res.Time = DateTime.UtcNow;
            return StatusCode(res.StatusCode, res);
        }

        [HttpPost("unlock")]
        public ActionResult<BaseDto> MomentaryUnlock([FromBody] MomentUnlockAcrDto dto)
        {
            BaseDto res = new BaseDto();
            if (!_acrService.MomentaryUnlock(dto.ScpMac, dto.AcrNo))
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

        [HttpGet("reader/mode")]
        public ActionResult<BaseDto<List<AcsRdrModeDto>>> GetAccessReaderMode()
        {
            BaseDto<List<AcsRdrModeDto>> res = new BaseDto<List<AcsRdrModeDto>>();
            List<AcsRdrModeDto> dtos = _acrService.GetAcsRdrModeList();

            if (dtos.Count > 0)
            {
                res.StatusDesc = "OK";
                res.StatusCode = 200;
                res.Time = DateTime.UtcNow;
                res.Content = dtos;
                return res;
            }
            res.Content = dtos;
            res.StatusDesc = "Internal Error";
            res.StatusCode = 500;
            res.Time = DateTime.UtcNow;
            return res;
        }

        [HttpGet("strike/mode")]
        public ActionResult<BaseDto<List<StrikeModeDto>>> GetStrikeModeList()
        {
            BaseDto<List<StrikeModeDto>> res = new BaseDto<List<StrikeModeDto>>();
            List<StrikeModeDto> dtos = _acrService.GetStrikeMode();
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
            res.StatusDesc = "Record is Empty";
            return Ok(res);
        }

        [HttpGet("mode")]
        public ActionResult<BaseDto<List<AcrModeDto>>> GetAcrModeList()
        {
            BaseDto<List<AcrModeDto>> res = new BaseDto<List<AcrModeDto>>();
            List<AcrModeDto> dtos = _acrService.GetAcrModeList();
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
            res.StatusDesc = "Record is Empty";
            return Ok(res);
        }

        [HttpGet("apb/mode")]
        public ActionResult<BaseDto<List<ApbModeDto>>> GetApbModeList()
        {
            BaseDto<List<ApbModeDto>> res = new BaseDto<List<ApbModeDto>>();
            List<ApbModeDto> dtos = _acrService.GetApbModeList();
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
            res.StatusDesc = "Record is Empty";
            return Ok(res);
        }

        [HttpGet("reader/{sio}")]
        public ActionResult<BaseDto<List<short>>> GetAvailableReader(short sio)
        {
            BaseDto<List<short>> res = new BaseDto<List<short>>();
            List<short> dtos = _acrService.GetAvailableReader(sio);
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
            res.StatusDesc = "Record is Empty";
            return Ok(res);
        }

        [HttpGet("status")]
        public ActionResult<BaseDto> GetAcrStatus([FromQuery] string ScpMac, short AcrNo)
        {
            BaseDto res = new BaseDto();
            if (!_acrService.GetAcrStatus(ScpMac, AcrNo))
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
        public ActionResult<BaseDto> RemoveAccessControlReader([FromBody] RemoveAcrDto dto)
        {
            BaseDto res = new BaseDto();
            if (!_acrService.RemoveAccessControlReader(dto.ScpMac, dto.AcrNo))
            {
                res.StatusCode = 500;
                res.Time = DateTime.UtcNow;
                res.StatusDesc = "Internal Server Error";
                return StatusCode(500, res);
            }

            res.StatusDesc = "OK";
            res.StatusCode = 200;
            res.Time = DateTime.UtcNow;
            return Ok(res);
        }

        [HttpPost("mode")]
        public ActionResult<BaseDto> ChangeACRMode([FromBody] ChangeACRModeDto dto)
        {
            BaseDto res = new BaseDto();
            if (!_acrService.ChangeACRMode(dto.ScpMac, dto.AcrNo, dto.Mode))
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



    }
}
