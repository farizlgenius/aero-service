using Aero.Application.DTOs;
using Aero.Application.Interface;
using Aero.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Aero.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ProcedureController(IProcedureService service) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<ResponseDto<IEnumerable<ProcedureDto>>>> GetAsync()
        {
            var res = await service.GetAsync();
            return Ok(res);
        }

        [HttpGet("/api/v1/{location}/[controller]")]
        public async Task<ActionResult<ResponseDto<IEnumerable<ProcedureDto>>>> GetByLocationIdAsync(short location)
        {
            var res = await service.GetByLocationIdAsync(location);
            return Ok(res);
        }

        [HttpGet("/api/v1/{location}/[controller]/pagination")]
        public async Task<ActionResult<ResponseDto<IEnumerable<ProcedureDto>>>> GetPaginationAsync([FromQuery]PaginationParamsWithFilter param,short location)
        {
            var res = await service.GetPaginationAsync(param,location);
            return Ok(res);
        }

        [HttpGet("type")]
        public async Task<ActionResult<ResponseDto<IEnumerable<Mode>>>> GetActionType()
        {
            var res = await service.GetActionType();
            return Ok(res);
        }

        [HttpPost]
        public async Task<ActionResult<ResponseDto<bool>>> CreateAsync(ProcedureDto dto)
        {
            var res = await service.CreateAsync(dto);
            return Ok(res);
        }

        [HttpPut]
        public async Task<ActionResult<ResponseDto<ProcedureDto>>> UpdateAsync(ProcedureDto dto)
        {
            var res = await service.UpdateAsync(dto);
            return Ok(res);
        }

        [HttpDelete("{componentId}")]
        public async Task<ActionResult<ResponseDto<bool>>> DeleteAsync(short componentId)
        {
            var res = await service.DeleteAsync(componentId);
            return Ok(res);
        }

    }
}
