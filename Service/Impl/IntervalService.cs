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
    public class IntervalService(AppDbContext context, IHelperService<Interval> helperService) : IIntervalService
    {
        public async Task<ResponseDto<IEnumerable<IntervalDto>>> GetAsync()
        {
            var dtos = await context.Intervals
                .AsNoTracking()
                .Include(s => s.Days)
                .Select(p => MapperHelper.IntervalToIntervalDto(p))
                .ToArrayAsync();
            
            if (dtos.Count() == 0) return ResponseHelper.NotFoundBuilder<IEnumerable<IntervalDto>>();
            return ResponseHelper.SuccessBuilder<IEnumerable<IntervalDto>>(dtos);
        }
        public async Task<ResponseDto<IntervalDto>> GetByIdAsync(short Id)
        {
            var dto = await context.Intervals
                .AsNoTracking()
                .Include(s => s.Days)
                .Where(x => x.Id == Id)
                .Select(p => MapperHelper.IntervalToIntervalDto(p))
                .FirstOrDefaultAsync();

            if(dto is null) return ResponseHelper.NotFoundBuilder<IntervalDto>();
            return ResponseHelper.SuccessBuilder(dto);
        }
        public async Task<ResponseDto<bool>> CreateAsync(CreateIntervalDto dto)
        {

            if (await context.Intervals.AnyAsync(p => 
            p.StartTime == dto.StartTime && 
            p.EndTime == dto.EndTime && 
            p.Days.Sunday == dto.Days.Sunday &&
            p.Days.Monday == dto.Days.Monday &&
            p.Days.Tuesday == dto.Days.Tuesday &&
            p.Days.Wednesday == dto.Days.Wednesday &&
            p.Days.Thursday == dto.Days.Thursday &&
            p.Days.Friday == dto.Days.Friday &&
            p.Days.Saturday == dto.Days.Saturday
            )) return ResponseHelper.Duplicate<bool>();

            var componentId = await helperService.GetLowestUnassignedNumberNoLimitAsync<Interval>(context);

            var interval = MapperHelper.CreateToInterval(dto, componentId, DaysInWeekToString(dto.Days));
            context.Intervals.Add(interval);
            await context.SaveChangesAsync();
            return ResponseHelper.SuccessBuilder(true);
        }

        public async Task<ResponseDto<bool>> DeleteAsync(short component)
        {
            var exists = await context.Intervals.AsNoTracking().AnyAsync(x => x.ComponentId == component);
            if (!exists) return ResponseHelper.NotFoundBuilder<bool>();
            var link = await context.TimeZoneIntervals.AsNoTracking().Where(x => x.IntervalId == component).AnyAsync();
            if (link) return ResponseHelper.FoundReferenceBuilder<bool>();

            // DeleteAsync using a lightweight tracked entity
            var interval = new Interval { Id = await context.Intervals.Where(x => x.ComponentId == component).Select(x => x.Id).FirstOrDefaultAsync() };
            context.Intervals.Attach(interval);
            context.Intervals.Remove(interval);
            await context.SaveChangesAsync();

            return ResponseHelper.SuccessBuilder(true);
        }

        public async Task<ResponseDto<IntervalDto>> UpdateAsync(IntervalDto dto)
        {
            var interval = await context.Intervals.Include(p => p.Days).FirstOrDefaultAsync(p => p.ComponentId == dto.ComponentId);
            if (interval is null) return ResponseHelper.NotFoundBuilder<IntervalDto>();


            context.Intervals.Update(MapperHelper.IntervalDtoMapInterval(dto,interval));
            await context.SaveChangesAsync();
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



    }
}
