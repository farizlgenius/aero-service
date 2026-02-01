
using Aero.Application.DTOs;
using Aero.Application.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Aero.Api.Controllers.V1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class TransactionController(ITransactionService transactionService) : ControllerBase
    {

        [HttpGet]
        public async Task<ActionResult<ResponseDto<PaginationDto>>> GetEvent([FromQuery] PaginationParams paginationParams)
        {
            var res = await transactionService.GetPageTransactionWithCountAsync(paginationParams);
            return Ok(res);
        }

        [HttpPost("{mac}")]
        public async Task<ActionResult<ResponseDto<bool>>> SetTranIndexAsync(string mac)
        {
            var res = await transactionService.SetTranIndexAsync(mac);
            return Ok(res);
        }
    }
}
