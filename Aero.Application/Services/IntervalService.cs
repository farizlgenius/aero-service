using System.Net;
using Aero.Api.Constants;
using Aero.Application.Constants;
using Aero.Application.DTOs;
using Aero.Application.Helpers;
using Aero.Application.Interface;
using Aero.Application.Interfaces;
using Aero.Application.Mapper;
using Aero.Domain.Interface;
using Aero.Domain.Interfaces;


namespace Aero.Application.Services
{
    public class IntervalService(IQIntervalRepository qInterval,IIntervalRepository rInterval,IQHwRepository qHw,ITzCommand tz,IQTzRepository qTz) : IIntervalService
    {
        public async Task<ResponseDto<IEnumerable<IntervalDto>>> GetAsync()
        {
            var dtos = await qInterval.GetAsync();
            
            return ResponseHelper.SuccessBuilder<IEnumerable<IntervalDto>>(dtos);
        }
        public async Task<ResponseDto<IntervalDto>> GetByIdAsync(short Id)
        {
            var dto = await qInterval.GetByComponentIdAsync(Id);

            if(dto is null) return ResponseHelper.NotFoundBuilder<IntervalDto>();
            return ResponseHelper.SuccessBuilder(dto);
        }
        public async Task<ResponseDto<IEnumerable<IntervalDto>>> GetByLocationAsync(short location)
        {
            var dtos = await qInterval.GetByLocationIdAsync(location);
            return ResponseHelper.SuccessBuilder<IEnumerable<IntervalDto>>(dtos);
        }
        public async Task<ResponseDto<bool>> CreateAsync(IntervalDto dto)
        {

            if (await qInterval.IsAnyOnEachDays(dto)) return ResponseHelper.Duplicate<bool>();

            var componentId = await qInterval.GetLowestUnassignedNumberAsync(10);

            var status = await rInterval.AddAsync(IntervalMapper.ToDomain(dto));

            if(status <= 0) return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.SAVE_DATABASE_UNSUCCESS,[]);

            return ResponseHelper.SuccessBuilder(true);
        }

        public async Task<ResponseDto<bool>> DeleteAsync(short component)
        {

            if (!await qInterval.IsAnyByComponentId(component)) return ResponseHelper.NotFoundBuilder<bool>();
            
            if(await qInterval.IsAnyReferenceByComponentAsync(component)) return ResponseHelper.FoundReferenceBuilder<bool>();
        
            // DeleteAsync using a lightweight tracked entity
            var status = await rInterval.DeleteByComponentIdAsync(component);

            if(status <= 0) return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.DELETE_DATABASE_UNSUCCESS,[]);

            return ResponseHelper.SuccessBuilder(true);
        }

        public async Task<ResponseDto<IntervalDto>> UpdateAsync(IntervalDto dto)
        {
            // Update is need to send the time zone command again 
            List<string> errors = new List<string>();

            if (!await qInterval.IsAnyByComponentId(dto.ComponentId)) return ResponseHelper.NotFoundBuilder<IntervalDto>();

            var status = await rInterval.UpdateAsync(IntervalMapper.ToDomain(dto));

            if(status <= 0) return ResponseHelper.UnsuccessBuilder<IntervalDto>(ResponseMessage.UPDATE_RECORD_UNSUCCESS,[]); 

            var hws = await qHw.GetComponentIdByLocationIdAsync(dto.LocationId);

            var ids = await qInterval.GetTimezoneIntervalIdByIntervalComponentIdAsync(dto.ComponentId);

            foreach (var id in ids)
            {
                foreach (var tzs in ids)
                {

                    var t = await qTz.GetByComponentIdAsync(tzs);
                    var a = await qInterval.GetIntervalFromTimezoneComponentIdAsync(tzs);

                    long active = UtilitiesHelper.DateTimeToElapeSecond(t.ActiveTime);
                    long deactive = UtilitiesHelper.DateTimeToElapeSecond(t.DeactiveTime);
                    if (!tz.ExtendedTimeZoneActSpecification(id,t,a.ToList(),(int)active,(int)deactive))
                    {
                        errors.Add(MessageBuilder.Unsuccess(await qHw.GetMacFromComponentAsync(id),Command.TIMEZONE_SPEC));
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
