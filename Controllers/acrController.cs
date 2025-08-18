using HIDAeroService.Dto;
using HIDAeroService.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HIDAeroService.Controllers
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
        public ActionResult<GenericDto<List<ACRDto>>> GetACRList()
        {
            GenericDto<List<ACRDto>> res = new GenericDto<List<ACRDto>>();
            List<ACRDto> dtos = _acrService.GetACRList();
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

        [HttpGet("{ip}")]
        public ActionResult<GenericDto<List<ACRDto>>> GetACRListByIp(string ip)
        {
            GenericDto<List<ACRDto>> res = new GenericDto<List<ACRDto>>();
            List<ACRDto> dtos = _acrService.GetACRListByIp(ip);
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
        public ActionResult<GenericDto> Create([FromForm] AddACRDto dto)
        {
            GenericDto res = new GenericDto();
            if (!_acrService.CreateAccessControlReaderForCreate(dto))
            {
                res.StatusCode = 500;
                res.StatusDesc = "Internal Error";
                res.Time = DateTime.UtcNow;
                return res;
            }
            res.StatusCode = 201;
            res.StatusDesc = "OK";
            res.Time = DateTime.UtcNow;
            return res;
        }

        [HttpPost("unlock")]
        public ActionResult<GenericDto> MomentaryUnlock([FromForm]string ScpIp,short AcrNo)
        {
            GenericDto res = new GenericDto();
            if(!_acrService.MomentaryUnlock(ScpIp, AcrNo))
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
        public ActionResult<GenericDto<List<AcsRdrModeDto>>> GetAccessReaderMode()
        {
            GenericDto<List<AcsRdrModeDto>> res = new GenericDto<List<AcsRdrModeDto>>();
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
        public ActionResult<GenericDto<List<StrikeModeDto>>> GetStrikeModeList()
        {
            GenericDto<List<StrikeModeDto>> res = new GenericDto<List<StrikeModeDto>>();
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
        public ActionResult<GenericDto<List<ACRModeDto>>> GetAcrModeList()
        {
            GenericDto<List<ACRModeDto>> res = new GenericDto<List<ACRModeDto>>();
            List<ACRModeDto> dtos = _acrService.GetAcrModeList();
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
        public ActionResult<GenericDto<List<ApbModeDto>>> GetApbModeList()
        {
            GenericDto<List<ApbModeDto>> res = new GenericDto<List<ApbModeDto>>();
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
        public ActionResult<GenericDto<List<short>>> GetAvailableReader(short sio)
        {
            GenericDto<List<short>> res = new GenericDto<List<short>>();
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
        public ActionResult<GenericDto> GetAcrStatus([FromQuery] string ScpIp, short AcrNo)
        {
            GenericDto res = new GenericDto();
            if (!_acrService.GetAcrStatus(ScpIp, AcrNo))
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
        public ActionResult<GenericDto> RemoveAccessControlReader([FromForm] string ScpIp, short AcrNumber)
        {
            GenericDto res = new GenericDto();
            if (!_acrService.RemoveAccessControlReader(ScpIp, AcrNumber))
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
        public ActionResult<GenericDto> ChangeACRMode([FromBody] ChangeACRModeDto dto)
        {
            GenericDto res = new GenericDto();
            if (!_acrService.ChangeACRMode(dto.ScpIp, dto.AcrNo, dto.Mode))
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
