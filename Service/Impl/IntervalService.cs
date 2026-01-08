using HIDAeroService.Aero.CommandService;
using HIDAeroService.Aero.CommandService.Impl;
using HIDAeroService.AeroLibrary;
using HIDAeroService.Constant;
using HIDAeroService.Constants;
using HIDAeroService.Data;
using HIDAeroService.DTO;
using HIDAeroService.DTO.Interval;
using HIDAeroService.Entity;
using HIDAeroService.Entity.Interface;
using HIDAeroService.Helpers;
using HIDAeroService.Mapper;
using HIDAeroService.Utility;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.EntityFrameworkCore;
using MiNET.Entities;
using MiNET.Worlds;
using System.ComponentModel;
using System.Net;


namespace HIDAeroService.Service.Impl
{
    public class IntervalService(AppDbContext context, IHelperService<Interval> helperService,ITimeZoneCommandService timeZoneCommandService) : IIntervalService
    {
        public async Task<ResponseDto<IEnumerable<IntervalDto>>> GetAsync()
        {
            var dtos = await context.interval
                .AsNoTracking()
                .Include(s => s.days)
                .Select(p => MapperHelper.IntervalToDto(p))
                .ToArrayAsync();
            
            return ResponseHelper.SuccessBuilder<IEnumerable<IntervalDto>>(dtos);
        }
        public async Task<ResponseDto<IntervalDto>> GetByIdAsync(short Id)
        {
            var dto = await context.interval
                .AsNoTracking()
                .Include(s => s.days)
                .Where(x => x.id == Id)
                .Select(p => MapperHelper.IntervalToDto(p))
                .FirstOrDefaultAsync();

            if(dto is null) return ResponseHelper.NotFoundBuilder<IntervalDto>();
            return ResponseHelper.SuccessBuilder(dto);
        }
        public async Task<ResponseDto<IEnumerable<IntervalDto>>> GetByLocationAsync(short location)
        {
            var dtos = await context.interval
                .AsNoTracking()
                .Include(s => s.days)
                .Where(x => x.location_id == location)
                .Select(x => MapperHelper.IntervalToDto(x))
                .ToArrayAsync();

            return ResponseHelper.SuccessBuilder<IEnumerable<IntervalDto>>(dtos);
        }
        public async Task<ResponseDto<bool>> CreateAsync(CreateIntervalDto dto)
        {

            if (await context.interval.AnyAsync(p => 
            p.start_time == dto.StartTime && 
            p.end_time == dto.EndTime && 
            p.days.sunday == dto.Days.Sunday &&
            p.days.monday == dto.Days.Monday &&
            p.days.tuesday == dto.Days.Tuesday &&
            p.days.wednesday == dto.Days.Wednesday &&
            p.days.thursday == dto.Days.Thursday &&
            p.days.friday == dto.Days.Friday &&
            p.days.saturday == dto.Days.Saturday
            )) return ResponseHelper.Duplicate<bool>();

            var componentId = await helperService.GetLowestUnassignedNumberNoLimitAsync<Interval>(context);

            var interval = MapperHelper.CreateToInterval(dto, componentId, DaysInWeekToString(dto.Days));
            context.interval.Add(interval);
            await context.SaveChangesAsync();
            return ResponseHelper.SuccessBuilder(true);
        }

        public async Task<ResponseDto<bool>> DeleteAsync(short component)
        {
            var en = await context.interval
                .Include(x => x.timezone_intervals)
                .OrderBy(x => x.id)
                .Where(x => x.component_id == component)
                .FirstOrDefaultAsync();

            if (en is null) return ResponseHelper.NotFoundBuilder<bool>();
            
            var link = await context.interval
                .AsNoTracking()
                .AnyAsync(x => x.component_id == component && x.timezone_intervals.Any());
            if (link) return ResponseHelper.FoundReferenceBuilder<bool>();

            // DeleteAsync using a lightweight tracked entity
            context.timezone_interval.RemoveRange(en.timezone_intervals);
            context.interval.Remove(en);
            await context.SaveChangesAsync();

            return ResponseHelper.SuccessBuilder(true);
        }

        public async Task<ResponseDto<IntervalDto>> UpdateAsync(IntervalDto dto)
        {
            // Update is need to send the time zone command again 
            List<string> errors = new List<string>();
            var en = await context.interval
                .Include(x => x.days)
                .Include(x => x.timezone_intervals)
                .OrderBy(x => x.id)
                .Where(x => x.component_id == dto.ComponentId)
                .FirstOrDefaultAsync();

            if (en is null) return ResponseHelper.NotFoundBuilder<IntervalDto>();

            MapperHelper.UpdateInterval(en, dto);

            context.interval.Update(en);
            await context.SaveChangesAsync();

            var hws = await context.hardware
                .AsNoTracking()
                .Where(x => x.location_id == en.location_id)
                .Select(x => x.component_id)
                .ToArrayAsync();

            foreach (var id in hws)
            {
                foreach (var tzs in en.timezone_intervals)
                {
                    var tz = await context.timezone
                        .AsNoTracking()
                        .Include(x => x.timezone_intervals)
                        .ThenInclude(x => x.interval)
                        .OrderBy(x => x.id)
                        .Where(x => x.component_id == tzs.timezone_id)
                        .FirstOrDefaultAsync();

                    long active = helperService.DateTimeToElapeSecond(tz.active_time);
                    long deactive = helperService.DateTimeToElapeSecond(tz.deactive_time);
                    if (!await timeZoneCommandService.ExtendedTimeZoneActSpecificationAsync(id,tz,tz.timezone_intervals.Select(x => x.interval).ToList(),(int)active,(int)deactive))
                    {
                        errors.Add(MessageBuilder.Unsuccess(await helperService.GetMacFromIdAsync(id),Command.C3103));
                    }
                }
            }

            if (errors.Count > 0) ResponseHelper.UnsuccessBuilder<IntervalDto>(ResponseMessage.COMMAND_UNSUCCESS,errors);

            return ResponseHelper.SuccessBuilder(dto);
        }

         private string DaysInWeekToString(DaysInWeekDto days)
        {
            var map = new Dictionary<string, bool>{
                {"Sun",days.Sunday },
                {
                    "Mon",days.Monday
                },
                {
                    "Tue",days.Tuesday
                },
                {
                    "Wed",days.Wednesday
                },
                {
                    "Thu",days.Thursday
                },
                {
                    "Fri",days.Friday
                },
                {
                    "Sat",days.Saturday
                }
            };

            return string.Join(",",map.Where(x => x.Value).Select(x => x.Key));
        }

        private DaysInWeekDto StringToDaysInWeek(string daysString)
        {
            var dto = new DaysInWeekDto();
            if (string.IsNullOrWhiteSpace(daysString)) return dto;

            var parts = daysString.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                  .Select(p => p.Trim());

            foreach (var day in parts)
            {
                switch (day)
                {
                    case "Sun": dto.Sunday = true; break;
                    case "Mon": dto.Monday = true; break;
                    case "Tue": dto.Tuesday = true; break;
                    case "Wed": dto.Wednesday = true; break;
                    case "Thu": dto.Thursday = true; break;
                    case "Fri": dto.Friday = true; break;
                    case "Sat": dto.Saturday = true; break;
                }
            }

            return dto;
        }

        private int ConvertDayToBinary(DaysInWeekDto days)
        {
            int result = 0;
            result |= (days.Sunday ? 1 : 0) << 0;
            result |= (days.Monday ? 1 : 0) << 1;
            result |= (days.Tuesday ? 1 : 0) << 2;
            result |= (days.Wednesday ? 1 : 0) << 3;
            result |= (days.Thursday ? 1 : 0) << 4;
            result |= (days.Friday ? 1 : 0) << 5;
            result |= (days.Saturday ? 1 : 0) << 6;

            // Holiday
            //result |= 0 << 8;
            //result |= 0 << 9;
            //result |= 0 << 10;
            //result |= 0 << 11;
            //result |= 0 << 12;
            //result |= 0 << 13;
            //result |= 0 << 14;
            //result |= 0 << 15;
            return result;
        }

        private int ConvertTimeToEndMinute(string timeString)
        {
            // Parse "HH:mm"
            var time = TimeSpan.Parse(timeString);

            // Convert hours/minutes to minutes since 12:00 AM
            int startMinutes = time.Hours * 60 + time.Minutes;

            // Return the minute number at the *end* of this minute
            return startMinutes;
        }

        public async Task<ResponseDto<IEnumerable<ResponseDto<bool>>>> DeleteRangeAsync(List<short> components)
        {
            bool flag = true;
            List<ResponseDto<bool>> data = new List<ResponseDto<bool>>();
            foreach (var dto in components)
            {
                var re = await DeleteAsync(dto);
                if (re.code != HttpStatusCode.OK) flag = false;
                data.Add(re);
            }

            if (!flag) return ResponseHelper.UnsuccessBuilder<IEnumerable<ResponseDto<bool>>>(data);

            var res = ResponseHelper.SuccessBuilder<IEnumerable<ResponseDto<bool>>>(data);

            return res;
        }
    }
}
