
using Aero.Application.DTOs;
using Aero.Application.Interface;
using Aero.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aero.Api.Controllers.V1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class TransactionController(ITransactionService transactionService) : ControllerBase
    {

        [HttpGet("/api/v1/{location}/[controller]")]
        [Authorize]
        public async Task<ActionResult<ResponseDto<Pagination<TransactionDto>>>> GetPaginationAsync([FromQuery]PaginationParamsWithFilter paginationParams,short location)
        {
            var res = await transactionService.GetPageTransactionWithCountAndDateAndSearchAsync(paginationParams,location);
            return Ok(res);
        }

        //[HttpGet]
        //public async Task<ActionResult<ResponseDto<PaginationDto>>> GetEventWithDateFilter([FromQuery] PaginationParams paginationParams)
        //{
        //    var res = await transactionService.GetPageTransactionWithCountAsync(paginationParams);
        //    return Ok(res);
        //}

        [HttpPost("{mac}")]
        [Authorize]
        public async Task<ActionResult<ResponseDto<bool>>> SetTranIndexAsync(string mac)
        {
            var res = await transactionService.SetTranIndexAsync(mac);
            return Ok(res);
        }

        [HttpGet("source")]
        public async Task<ActionResult<ResponseDto<IEnumerable<Mode>>>> GetSourceAsync()
        {
            var res = await transactionService.GetSourceAsync();
            return Ok(res);
        }

        [HttpGet("device/{source}")]
        public async Task<ActionResult<ResponseDto<IEnumerable<Mode>>>> GetDeviceAsync(int source)
        {
            var res = await transactionService.GetDeviceAsync(source);
            return Ok(res);
        }
    }
}
