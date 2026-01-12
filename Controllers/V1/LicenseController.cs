using AeroService.DTO;
using AeroService.DTO.License;
using AeroService.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AeroService.Controllers.V1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class LicenseController(ILicenseService service) : ControllerBase
    {
        [HttpPost("trusted")]
        public async Task<IActionResult> TrustServerAsync()
        {
            var res = await service.TrustServerAsync(dto);
            return Ok(res);
        }

        [HttpGet("identity")]
        public async Task<ActionResult<ResponseDto<MachineFingerPrintDto>>> GetMachineId()
        {
            var res = await service.GetMachineIdAsync();
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
