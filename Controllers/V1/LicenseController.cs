using AeroService.DTO;
using AeroService.DTO.License;
using AeroService.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AeroService.Controllers.V1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class LicenseController(ILicenseService licenseService) : ControllerBase
    {
        [HttpGet("identity")]
        public async Task<ActionResult<ResponseDto<MachineFingerPrintDto>>> GetMachineId()
        {
            var res = await licenseService.GetMachineIdAsync();
            return Ok(res);
        }

        [HttpPost]
        public async Task<ActionResult<ResponseDto<bool>>> CreateAsync()
        {
            var res = await licenseService.AddLicenseAsync();
            return Ok(res);
        }
    }
}
