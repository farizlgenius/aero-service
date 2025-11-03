using HIDAeroService.DTO;
using HIDAeroService.Hubs;
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
    public class EventController(EventService eventService) : ControllerBase
    {

        [HttpGet]
        public async Task<ActionResult<ResponseDto<PaginationDto<List<EventDto>>>>> GetEvent([FromQuery] PaginationParams paginationParams)
        {
            var (data, totalCount) = await eventService.GetPagedEventsWithCountAsync(paginationParams);
            PaginationDto<List<EventDto>> pagination = new PaginationDto<List<EventDto>>();
            pagination.Data = data;
            PaginationData page = new PaginationData();
            page.TotalCount = totalCount;
            page.PageNumber = paginationParams.PageNumber;
            page.PageSize = paginationParams.PageSize;
            page.TotalPage = (int)Math.Ceiling(totalCount / (double)paginationParams.PageSize);
            pagination.Page = page;

            ResponseDto<PaginationDto<List<EventDto>>> p = new ResponseDto<PaginationDto<List<EventDto>>>();
            p.TimeStamp = DateTime.UtcNow;
            p.Message = "OK";
            p.Code = HttpStatusCode.OK;
            p.Data = pagination;

            return Ok(p);
        }

        [HttpPost("{mac}")]
        public async Task<ActionResult<ResponseDto<bool>>> SetTranIndexAsync(string mac)
        {
            var res = await eventService.SetTranIndexAsync(mac);
            return Ok(res);
        }
    }
}
