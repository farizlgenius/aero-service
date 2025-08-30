using HID.Aero.ScpdNet.Wrapper;
using HIDAeroService.AeroLibrary;
using HIDAeroService.Data;
using HIDAeroService.Dto;
using HIDAeroService.Entity;
using HIDAeroService.Hubs;
using HIDAeroService.Mapper;
using HIDAeroService.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;

namespace HIDAeroService.Service
{
    public sealed class EventService 
    {
        private readonly AppDbContext _context;
        private readonly IHubContext<EventHub> _hub;
        public EventService(AppDbContext appDbContext, IHubContext<EventHub> hub) 
        {
            _context = appDbContext;
            _hub = hub;
        }

        public async Task<(List<EventDto> Data, int TotalCount)> GetPagedEventsWithCountAsync(PaginationParams paginationParams)
        {
            var query = _context.ArEvents.AsQueryable();

            var totalCount = await query.CountAsync();

            List<EventDto> events = new List<EventDto>();

            var data = await query
                .OrderByDescending(u => u.CreatedDate)
                .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
                .Take(paginationParams.PageSize)
                .ToListAsync();

            foreach(ArEvent d in data)
            {
                events.Add(MapperHelper.EventToEventDto(d));
            }

            return (events, totalCount);
        }

        public void SaveToDatabase(SCPReplyMessage message, string tranCodeDesc,string? additional = "")
        {
            try
            {
                var result = _hub.Clients.All.SendAsync("eventTrig", true);
                string typeDesc = Description.GetTypeDesc(message.tran.tran_type);
                string sourceDesc = Description.GetSourceDesc(message.tran.source_type);
                ArEvent tran = new ArEvent();
                string[] dt = Utility.UnixToDateTimeParts(message.tran.time);
                tran.Date = dt[0];
                tran.Time = dt[1];
                tran.Serialnumber = message.tran.ser_num;
                tran.Source = sourceDesc;
                tran.SourceNo = message.tran.source_number.ToString();
                tran.Type = typeDesc;
                tran.TransactionCode = message.tran.tran_code;
                tran.Description = tranCodeDesc;
                tran.Additional = additional;
                tran.CreatedDate = DateTime.Now;
                tran.UpdatedDate = DateTime.Now;
                _context.ArEvents.Add(tran);
                _context.SaveChanges();
                
            }
            catch (Exception ex) 
            {
                Console.WriteLine(ex.ToString());
            }
            
        }


    }
}
