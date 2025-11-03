using HID.Aero.ScpdNet.Wrapper;
using HIDAeroService.AeroLibrary;
using HIDAeroService.Constant;
using HIDAeroService.Constants;
using HIDAeroService.Data;
using HIDAeroService.DTO;
using HIDAeroService.Entity;
using HIDAeroService.Helpers;
using HIDAeroService.Hubs;
using HIDAeroService.Mapper;
using HIDAeroService.Utility;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;

namespace HIDAeroService.Service.Impl
{
    public sealed class EventService(AppDbContext context, IHubContext<EventHub> hub,AeroCommand command,IHelperService helperService) : IEventService
    {
        public async Task<(List<EventDto> Data, int TotalCount)> GetPagedEventsWithCountAsync(PaginationParams paginationParams)
        {
            var query = context.ArEvents.AsQueryable();

            var totalCount = await query.CountAsync();

            List<EventDto> events = new List<EventDto>();

            var data = await query
                .OrderByDescending(u => u.CreatedDate)
                .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
                .Take(paginationParams.PageSize)
                .ToListAsync();

            foreach (ArEvent d in data)
            {
                events.Add(MapperHelper.EventToEventDto(d));
            }

            return (events, totalCount);
        }

        public void SaveToDatabase(SCPReplyMessage message, string tranCodeDesc, string? additional = "")
        {
            var result = hub.Clients.All.SendAsync("eventTrig", true);
            string typeDesc = Description.GetTypeDesc(message.tran.tran_type);
            string sourceDesc = Description.GetSourceDesc(message.tran.source_type);
            ArEvent tran = new ArEvent();
            string[] dt = UtilityHelper.UnixToDateTimeParts(message.tran.time);
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
            context.ArEvents.Add(tran);
            context.SaveChanges();

        }

        public async Task<ResponseDto<bool>> SetTranIndexAsync(string mac)
        {
            var id = await helperService.GetIdFromMacAsync(mac);
            if (id == 0) return ResponseHelper.NotFoundBuilder<bool>();
            if (!await command.SetTransactionLogIndexAsync(id,true))
            {
                return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.COMMAND_UNSUCCESS,MessageBuilder.Unsuccess(mac,Command.C303));
            }

            return ResponseHelper.SuccessBuilder(true);
        }
    }
}
