

using Aero.Application.DTOs;
using Aero.Application.Interface;
using Aero.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Aero.Api.Controllers.V1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class TriggerController(ITriggerService service) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<ResponseDto<IEnumerable<TriggerDto>>>> GetAsync()
        {
            var res = await service.GetAsync();
             return Ok(res);
        }

        [HttpGet("/api/v1/{location}/[controller]")]
        public async Task<ActionResult<ResponseDto<IEnumerable<TriggerDto>>>> GetByLocationId(short location)
        {
            var res = await service.GetByLocationId(location); 
            return Ok(res);
        }

        [HttpGet("/api/v1/{location}/[controller]/pagination")]
        public async Task<ActionResult<ResponseDto<Pagination<TriggerDto>>>> GetPaginationAsync([FromQuery]PaginationParamsWithFilter param,short location)
        {
            var res = await service.GetPaginationAsync(param,location);
            return Ok(res);
        }

        [HttpGet("command")]
        public async Task<ActionResult<ResponseDto<IEnumerable<Mode>>>> GetCommandAsync()
        {
            var res = await service.GetCommandAsync();
            return Ok(res);
        }

        [HttpGet("source")]
        public async Task<ActionResult<ResponseDto<IEnumerable<Mode>>>> GetSourceTypeAsync()
        {
            var res = await service.GetSourceTypeAsync();
            return Ok(res);
        }

        [HttpGet("tran/{source}")]
        public async Task<ActionResult<ResponseDto<IEnumerable<Mode>>>> GetTypeBySourceAsync(short source)
        {
            var res = await service.GetTypeBySourceAsync(source);
            return Ok(res);
        }

        [HttpGet("code/{tran}")]
        public async Task<ActionResult<ResponseDto<IEnumerable<Mode>>>> GetCodeByTranAsync(short tran)
        {
            var res = await service.GetCodeByTranAsync(tran);
            return Ok(res);
        }

        [HttpGet("/api/v1/{location}/[controller]/device/{source}")]
        public async Task<ActionResult<ResponseDto<IEnumerable<Mode>>>> GetDeviceBySource(short location, short source)
        {
            var res = await service.GetDeviceBySourceAsync(location,source);
            return Ok(res);
        }

        [HttpPost]
        public async Task<ActionResult<ResponseDto<bool>>> CreateAsync(TriggerDto dto)
        {
            var res = await service.CreateAsync(dto);
            return Ok(res);
        }

        [HttpPut]
        public async Task<ActionResult<ResponseDto<TriggerDto>>> UpdateAsync(TriggerDto dto)
        {
            var res = await service.UpdateAsync(dto);
            return Ok(res);
        }

        [HttpDelete]
        public async Task<ActionResult<ResponseDto<bool>>> DeleteAsync(string Mac,short ComponentId)
        {
            var res = await service.DeleteAsync(Mac,ComponentId);
            return Ok(res);
        }


    }
}
