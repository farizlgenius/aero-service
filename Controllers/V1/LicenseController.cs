using AeroService.Data;
using AeroService.DTO;
using AeroService.DTO.License;
using AeroService.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace AeroService.Controllers.V1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class LicenseController(ILicenseService service,IOptions<AppConfigSettings> options) : ControllerBase
    {
        private readonly AppConfigSettings settings = options.Value;

        [HttpPost("exchange")]
        public async Task<IActionResult> ExchangeAsync(TrustServerDto dto)
        {
            var res = await service.ExchangeAsync(dto);
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
