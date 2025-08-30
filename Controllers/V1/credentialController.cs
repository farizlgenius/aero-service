using HID.Aero.ScpdNet.Wrapper;
using HIDAeroService.Constants;
using HIDAeroService.Data;
using HIDAeroService.Dto;
using HIDAeroService.Dto.Credential;
using HIDAeroService.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace HIDAeroService.Controllers.V1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class credentialController : ControllerBase
    {

        private readonly AeroLibMiddleware _config;
        private readonly CredentialService _credentialService;
        public credentialController(AeroLibMiddleware config, CredentialService credentialService)
        {
            _credentialService = credentialService;
            _config = config;
        }


        [HttpGet("all")]
        public ActionResult<BaseDto<List<CardHolderDto>>> GetCredentialList()
        {
            BaseDto<List<CardHolderDto>> res = new BaseDto<List<CardHolderDto>>();
            List<CardHolderDto> content = _credentialService.GetCardHolderList();
            if (content.Count > 0)
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
        public ActionResult<BaseDto> ScanCardTrigger([FromBody] ScanCardDto dto)
        {
            BaseDto res = new BaseDto();
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
        public ActionResult<BaseDto> Create(CreateCardHolderDto dto)
        {
            BaseDto res = new BaseDto();
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

        [HttpDelete("remove/{reference}")]
        public ActionResult<BaseDto> Delete(string reference)
        {
            BaseDto res = new BaseDto();
            res.StatusDesc = _credentialService.RemoveCardHolder(reference);
            if (!res.StatusDesc.Equals(Constants.ConstantsHelper.COMMAND_SUCCESS))
            {
                res.StatusCode = Constants.ConstantsHelper.INTERNAL_ERROR_CODE;
                res.StatusDesc = Constants.ConstantsHelper.INTERNAL_ERROR;
                res.Time = DateTime.UtcNow;
                return base.Ok(res);
            }

            res.StatusDesc = Constants.ConstantsHelper.REMOVE_SUCCESS;
            res.StatusCode = Constants.ConstantsHelper.SUCCESS_CODE;
            res.Time = DateTime.UtcNow;
            return Ok(res);
        }

    }
}
