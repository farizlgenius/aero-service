using HIDAeroService.Dto;
using HIDAeroService.Hubs;
using HIDAeroService.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;

namespace HIDAeroService.Controllers.V1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class eventController : ControllerBase
    {
        private readonly IHubContext<EventHub> _hub;
        private readonly EventService _eventService;
        public eventController(EventService eventService, IHubContext<EventHub> hub)
        {
            _hub = hub;
            _eventService = eventService;
        }



        [HttpGet]
        public async Task<ActionResult<BaseDto<PaginationDto<List<EventDto>>>>> GetEvent([FromQuery] PaginationParams paginationParams)
        {
            var (data, totalCount) = await _eventService.GetPagedEventsWithCountAsync(paginationParams);
            PaginationDto<List<EventDto>> pagination = new PaginationDto<List<EventDto>>();
            pagination.Data = data;
            PaginationData page = new PaginationData();
            page.TotalCount = totalCount;
            page.PageNumber = paginationParams.PageNumber;
            page.PageSize = paginationParams.PageSize;
            page.TotalPage = (int)Math.Ceiling(totalCount / (double)paginationParams.PageSize);
            pagination.Page = page;

            BaseDto<PaginationDto<List<EventDto>>> p = new BaseDto<PaginationDto<List<EventDto>>>();
            p.Time = DateTime.UtcNow;
            p.StatusDesc = "OK";
            p.StatusCode = 200;
            p.Content = pagination;

            return Ok(p);
        }
    }
}
