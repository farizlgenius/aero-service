using System.Net;
using Aero.Api.Constants;
using Aero.Application.Constants;
using Aero.Application.DTOs;
using Aero.Application.Helpers;
using Aero.Application.Interface;
using Aero.Application.Interfaces;
using Aero.Application.Mapper;
using Aero.Domain.Entities;
using Aero.Domain.Interface;
using Aero.Domain.Interfaces;


namespace Aero.Application.Services
{
    public class IntervalService(IIntervalRepository repo,IHwRepository hw,ITzCommand tz,ITzRepository tzRepo) : IIntervalService
    {
        public async Task<ResponseDto<IEnumerable<IntervalDto>>> GetAsync()
        {
            var dtos = await repo.GetAsync();
            
            return ResponseHelper.SuccessBuilder<IEnumerable<IntervalDto>>(dtos);
        }
        public async Task<ResponseDto<IntervalDto>> GetByIdAsync(int Id)
        {
            var dto = await repo.GetByIdAsync(Id);

            if(dto is null) return ResponseHelper.NotFoundBuilder<IntervalDto>();
            return ResponseHelper.SuccessBuilder(dto);
        }
        public async Task<ResponseDto<IEnumerable<IntervalDto>>> GetByLocationAsync(int location)
        {
            var dtos = await repo.GetByLocationIdAsync(location);
            return ResponseHelper.SuccessBuilder<IEnumerable<IntervalDto>>(dtos);
        }
        public async Task<ResponseDto<IntervalDto>> CreateAsync(CreateIntervalDto dto)
        {

            if (await repo.IsAnyOnEachDays(dto)) return ResponseHelper.Duplicate<IntervalDto>();

            var domain = new Aero.Domain.Entities.Interval(new Aero.Domain.Entities.DaysInWeek(dto.Days.Sunday,dto.Days.Monday,dto.Days.Tuesday,dto.Days.Wednesday,dto.Days.Thursday,dto.Days.Friday,dto.Days.Saturday),dto.DaysDetail,dto.Start,dto.End,dto.LocationId,dto.IsActive);

            var id = await repo.AddAsync(domain);

            if(id <= 0) return ResponseHelper.UnsuccessBuilder<IntervalDto>(ResponseMessage.SAVE_DATABASE_UNSUCCESS,[]);

            return ResponseHelper.SuccessBuilder(await repo.GetByIdAsync(id));
        }

        public async Task<ResponseDto<IntervalDto>> DeleteAsync(int id)
        {
            var en = await repo.GetByIdAsync(id);
            if (en is null) return ResponseHelper.NotFoundBuilder<IntervalDto>();
            
            if(await repo.IsAnyReferenceByIdAsync(id)) return ResponseHelper.FoundReferenceBuilder<IntervalDto>();
        
            // DeleteAsync using a lightweight tracked entity
            var status = await repo.DeleteByIdAsync(id);

            if(status <= 0) return ResponseHelper.UnsuccessBuilder<IntervalDto>(ResponseMessage.DELETE_DATABASE_UNSUCCESS,[]);

            return ResponseHelper.SuccessBuilder(en);
        }

        public async Task<ResponseDto<IntervalDto>> UpdateAsync(IntervalDto dto)
        {
            // Update is need to send the time zone command again 
            List<string> errors = new List<string>();

            if (!await repo.IsAnyById(dto.Id)) return ResponseHelper.NotFoundBuilder<IntervalDto>();

            var status = await repo.UpdateAsync(IntervalMapper.ToDomain(dto));

            if(status <= 0) return ResponseHelper.UnsuccessBuilder<IntervalDto>(ResponseMessage.UPDATE_RECORD_UNSUCCESS,[]); 

            var hws = await hw.GetDriverIdByLocationIdAsync(dto.LocationId);

            var ids = await repo.GetTimezoneIntervalIdByIntervalIdAsync(dto.Id);

            foreach (var id in hws)
            {
                foreach (var tzs in ids)
                {

                    var t = await tzRepo.GetByIdAsync(tzs);
                    var tdomain = new Aero.Domain.Entities.TimeZone(t.DriverId,t.Name,t.Mode,t.Active,t.Deactive,t.LocationId,t.IsActive);

                    if (!tz.ExtendedTimeZoneActSpecification(id,tdomain))
                    {
                        errors.Add(MessageBuilder.Unsuccess(await hw.GetMacFromComponentAsync(id),Command.TIMEZONE_SPEC));
                    }
                }
            }

            if (errors.Count > 0) ResponseHelper.UnsuccessBuilder<IntervalDto>(ResponseMessage.COMMAND_UNSUCCESS,errors);

            return ResponseHelper.SuccessBuilder(dto);
        }

        // private string DaysInWeekToString(DaysInWeekDto days)
        //{
        //    var map = new Dictionary<string, bool>{
        //        {"Sun",days.Sunday },
        //        {
        //            "Mon",days.Monday
        //        },
        //        {
        //            "Tue",days.Tuesday
        //        },
        //        {
        //            "Wed",days.Wednesday
        //        },
        //        {
        //            "Thu",days.Thursday
        //        },
        //        {
        //            "Fri",days.Friday
        //        },
        //        {
        //            "Sat",days.Saturday
        //        }
        //    };

        //    return string.Join(",",map.Where(x => x.Value).Select(x => x.Key));
        //}

        //private DaysInWeekDto StringToDaysInWeek(string daysString)
        //{
        //    if (string.IsNullOrWhiteSpace(daysString)) return new DaysInWeekDto(false,false,false,false,false,false,false);

        //    var parts = daysString.Split(',', StringSplitOptions.RemoveEmptyEntries)
        //                          .Select(p => p.Trim());

        //    foreach (var day in parts)
        //    {
        //        switch (day)
        //        {
        //            case "Sun": dto.Sunday = true; break;
        //            case "Mon": dto.Monday = true; break;
        //            case "Tue": dto.Tuesday = true; break;
        //            case "Wed": dto.Wednesday = true; break;
        //            case "Thu": dto.Thursday = true; break;
        //            case "Fri": dto.Friday = true; break;
        //            case "Sat": dto.Saturday = true; break;
        //        }
        //    }

        //    return dto;
        //}

        //private int ConvertDayToBinary(DaysInWeekDto days)
        //{
        //    int result = 0;
        //    result |= (days.Sunday ? 1 : 0) << 0;
        //    result |= (days.Monday ? 1 : 0) << 1;
        //    result |= (days.Tuesday ? 1 : 0) << 2;
        //    result |= (days.Wednesday ? 1 : 0) << 3;
        //    result |= (days.Thursday ? 1 : 0) << 4;
        //    result |= (days.Friday ? 1 : 0) << 5;
        //    result |= (days.Saturday ? 1 : 0) << 6;

        //    // Holiday
        //    //result |= 0 << 8;
        //    //result |= 0 << 9;
        //    //result |= 0 << 10;
        //    //result |= 0 << 11;
        //    //result |= 0 << 12;
        //    //result |= 0 << 13;
        //    //result |= 0 << 14;
        //    //result |= 0 << 15;
        //    return result;
        //}

        //private int ConvertTimeToEndMinute(string timeString)
        //{
        //    // Parse "HH:mm"
        //    var time = TimeSpan.Parse(timeString);

        //    // Convert hours/minutes to minutes since 12:00 AM
        //    int startMinutes = time.Hours * 60 + time.Minutes;

        //    // Return the minute number at the *end* of this minute
        //    return startMinutes;
        //}

        public async Task<ResponseDto<IEnumerable<IntervalDto>>> DeleteRangeAsync(List<int> ids)
        {
            bool flag = true;
            List<IntervalDto> data = new List<IntervalDto>();
            foreach (var dto in ids)
            {
                var re = await DeleteAsync(dto);
                if (re.code != HttpStatusCode.OK) flag = false;
                if(re.data is not null) data.Add(re.data);
            }

            if (!flag) return ResponseHelper.UnsuccessBuilder<IEnumerable<IntervalDto>>(data);

            var res = ResponseHelper.SuccessBuilder<IEnumerable<IntervalDto>>(data);

            return res;
        }

        public async Task<ResponseDto<Pagination<IntervalDto>>> GetPaginationAsync(PaginationParamsWithFilter param, int location)
        {
            var res = await repo.GetPaginationAsync(param,location);
            return ResponseHelper.SuccessBuilder(res);
        }
    }
}
