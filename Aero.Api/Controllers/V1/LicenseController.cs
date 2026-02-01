using Aero.Api.Configuration;
using Aero.Application.DTOs;
using Aero.Application.Interface;
using Aero.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Aero.Api.Controllers.V1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class LicenseController(ILicenseService service,IOptions<AppSettings> options) : ControllerBase
    {
        private readonly AppSettings settings = options.Value;

        [HttpGet]
        public async Task<ActionResult<ResponseDto<bool>>> CheckLicenseAsync()
        {
            var res = await service.CheckLicenseAsync();
            return Ok(res);
        }

        [HttpGet("identity")]
        public async Task<ActionResult<ResponseDto<MachineFingerPrintDto>>> GetMachineId()
        {
            var res = await service.GetMachineIdAsync();
            return Ok(res);
        }

        [HttpPost("generate/demo")]
        public async Task<ActionResult<ResponseDto<bool>>> GenerateDemoLicenseAsync([FromBody] GenerateDemoRequest request)
        {
            // TO DO : Implement Demo License Generation
            var res = await service.GenerateDemoLicenseAsync(request);
            return Ok(res);
        }
       
        

        [HttpPost]
        public async Task<ActionResult<ResponseDto<bool>>> CreateAsync()
        {
            var res = await service.AddLicenseAsync();
            return Ok(res);
        }
    }
}
