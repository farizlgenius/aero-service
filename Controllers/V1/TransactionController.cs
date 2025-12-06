using HIDAeroService.DTO;
using HIDAeroService.DTO.Transactions;
using HIDAeroService.Hubs;
using HIDAeroService.Service;
using HIDAeroService.Service.Impl;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Net;

namespace HIDAeroService.Controllers.V1
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
