using HIDAeroService.DTO;
using HIDAeroService.DTO.License;
using HIDAeroService.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HIDAeroService.Controllers.V1
{
    [Route("api/[controller]")]
    [ApiController]
    public class LicenseController(ILicenseService licenseService) : ControllerBase
    {
        [HttpGet("identity")]
        public async Task<ActionResult<ResponseDto<MachineIdentityDto>>> GetMachineId()
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
