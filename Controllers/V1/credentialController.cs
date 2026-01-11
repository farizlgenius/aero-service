using HIDAeroService.DTO;
using HIDAeroService.DTO.Credential;
using HIDAeroService.DTO.Token;
using HIDAeroService.Entity;
using HIDAeroService.Service;
using Microsoft.AspNetCore.Mvc;

namespace HIDAeroService.Controllers.V1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CredentialController(ICredentialService credentialService) : ControllerBase
    {

        [HttpGet]
        public async Task<ActionResult<ResponseDto<IEnumerable<CredentialDto>>>> GetAsync()
        {
            var res = await credentialService.GetAsync();
            return Ok(res);
        }

        [HttpPost("scan")]
        public async Task<ActionResult<bool>> ScanCardAsync([FromBody] ScanCardDto dto) 
        {
            var res = await credentialService.ScanCardTrigger(dto);
            return Ok(res);
        }

        [HttpGet("{userid}")]
        public async Task<ActionResult<ResponseDto<CredentialDto>>> GetByComponentAsync(string userid)
        {
            var res = await credentialService.GetByUserId(userid);
            return Ok(res);
        }

        [HttpPost]
        public async Task<ActionResult<ResponseDto<CredentialDto>>> CreateAsync([FromBody] CredentialDto dto)
        {
            var res = await credentialService.CreateAsync(dto);
            return Ok(res);
        }

        [HttpPut]
        public async Task<ActionResult<ResponseDto<CredentialDto>>> UpdateAsync([FromBody] CredentialDto dto)
        {
            var res = await credentialService.UpdateAsync(dto);
            return Ok(res);
        }

        [HttpDelete]
        public async Task<ActionResult<ResponseDto<CredentialDto>>> DeleteAsync([FromBody] CredentialDto dto)
        {
            var res = await credentialService.DeleteAsync(dto);
            return Ok(res);
        }


        [HttpDelete("card")]
        public async Task<ActionResult<ResponseDto<bool>>> DeleteCardAsync([FromBody] DeleteCardDto dto) 
        {
            var res = await credentialService.DeleteCardAsync(dto);
            return Ok(res);
        }

        [HttpGet("flag")]
        public async Task<ActionResult<ResponseDto<ModeDto>>> GetCredentialFlagAsync()
        {
            var res = await credentialService.GetCredentialFlagAsync();
            return Ok(res);
        }
    }
}
