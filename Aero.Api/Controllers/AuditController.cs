using Aero.Application.DTOs;
using Aero.Application.Services;
using Aero.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Aero.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
     [Authorize]
    public class AuditController(AuditService service) : ControllerBase
    {
        [HttpGet("/api/{location}/[controller]")]
        public async Task<ActionResult<ResponseDto<Pagination<AuditDto>>>> GetPaginationAsync([FromQuery]PaginationParamsWithFilter paginationParams,short location)
        {
            var res = await service.GetPageTransactionWithCountAndDateAndSearchAsync(paginationParams,location);
            return Ok(res);
        }
    }
}
