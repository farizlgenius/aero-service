using HID.Aero.ScpdNet.Wrapper;
using HIDAeroService.Data;
using HIDAeroService.Dto;
using HIDAeroService.Dto.Credential;
using HIDAeroService.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace HIDAeroService.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class credentialController : ControllerBase
    {

        private readonly AppConfigData _config;
        private readonly CredentialService _credentialService;
        public credentialController(AppConfigData config,CredentialService credentialService)
        {
            _credentialService = credentialService;
            _config = config;
        }


        [HttpGet("all")]
        public ActionResult<GenericDto<List<CardHolderDto>>> GetCredentialList()
        {
            GenericDto<List<CardHolderDto>> res = new GenericDto<List<CardHolderDto>>();
            List<CardHolderDto> content = _credentialService.GetCardHolderList();
            if(content.Count > 0)
            {
                res.StatusDesc = "Ok";
                res.StatusCode = 200;
                res.Content = content;
                res.Time = DateTime.UtcNow;
                return Ok(res);
            }
            res.StatusCode = 200;
            res.StatusDesc = "Record Empty";
            res.Time = DateTime.UtcNow;
            res.Content = [];
            return Ok(res);

        }

        [HttpPost("scan")]
        public ActionResult<GenericDto> ScanCardTrigger([FromBody] ScanCardDto dto)
        {
            GenericDto res = new GenericDto();
            if (!_credentialService.ScanCardTrigger(dto))
            {
                res.StatusDesc = "Internal Error";
                res.StatusCode = 500;
                res.Time = DateTime.UtcNow;
                return Ok(res);
            }
            res.StatusDesc = "OK";
            res.StatusCode = 200;
            res.Time = DateTime.UtcNow;
            return Ok(res);
        }

        [HttpPost("add")]
        public ActionResult<GenericDto> Create(CreateCardHolderDto dto)
        {
            GenericDto res = new GenericDto();
            if (!_credentialService.CreateCardHolder(dto))
            {
                res.StatusDesc = "Can't Create Card Holder";
                res.StatusCode = 500;
                res.Time = DateTime.UtcNow;
                return Ok(res);
            }

            res.StatusDesc = "OK";
            res.StatusCode = 201;
            res.Time = DateTime.UtcNow;
            return Ok(res);
        }

    }
}
