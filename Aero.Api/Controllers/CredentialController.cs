using Aero.Application.DTOs;
using Aero.Application.Interface;
using Aero.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aero.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CredentialController(ICredentialService credentialService) : ControllerBase
    {

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<ResponseDto<IEnumerable<CredentialDto>>>> GetAsync()
        {
            var res = await credentialService.GetAsync();
            return Ok(res);
        }

        [HttpPost("scan")]
        [Authorize]
        public async Task<ActionResult<bool>> ScanCardAsync([FromBody] ScanCardDto dto) 
        {
            var res = await credentialService.ScanCardTrigger(dto);
            return Ok(res);
        }

        [HttpGet("{userid}")]
        [Authorize]
        public async Task<ActionResult<ResponseDto<CredentialDto>>> GetByComponentAsync(string userid)
        {
            var res = await credentialService.GetByUserId(userid);
            return Ok(res);
        }

        // [HttpPost]
        // public async Task<ActionResult<ResponseDto<CredentialDto>>> CreateAsync([FromBody] CredentialDto dto)
        // {
        //     var res = await credentialService.CreateAsync(dto);
        //     return Ok(res);
        // }

        // [HttpPut]
        // public async Task<ActionResult<ResponseDto<CredentialDto>>> UpdateAsync([FromBody] CredentialDto dto)
        // {
        //     var res = await credentialService.UpdateAsync(dto);
        //     return Ok(res);
        // }

        // [HttpDelete]
        // public async Task<ActionResult<ResponseDto<CredentialDto>>> DeleteAsync([FromBody] CredentialDto dto)
        // {
        //     var res = await credentialService.DeleteAsync(dto);
        //     return Ok(res);
        // }


        [HttpDelete("card")]
        [Authorize]
        public async Task<ActionResult<ResponseDto<bool>>> DeleteCardAsync([FromBody] DeleteCardDto dto) 
        {
            var res = await credentialService.DeleteCardAsync(dto);
            return Ok(res);
        }

        [HttpGet("flag")]
        [Authorize]
        public async Task<ActionResult<ResponseDto<Mode>>> GetCredentialFlagAsync()
        {
            var res = await credentialService.GetCredentialFlagAsync();
            return Ok(res);
        }
    }
}
