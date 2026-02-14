using Aero.Application.DTOs;
using Aero.Application.Helpers;
using Aero.Application.Interfaces;
using Aero.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Aero.Api.Controllers.V1
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiagnosticController(IDiagnosticService service) : ControllerBase
    {
        [HttpGet("api/{location}/[controller]")]
        public async Task<ActionResult<ResponseDto<Pagination<CommandAudit>>>> GetCommandStatusAsync([FromQuery] PaginationParamsWithFilter paginationParams, short location)
        {
            var res = await service.GetCommandStatusAsync(paginationParams,location);
            return Ok(res);
        }

        [HttpPost("command")]
        public async Task<ActionResult<ResponseDto<bool>>> CommandAsync(CommandDto command)
        {
            var res = await service.CommandAsync(command);
            return Ok(res);
        }
    }
}
