using AutoMapper;
using HIDAeroService.Dto.Interval;
using HIDAeroService.Dto.TimeZone;
using HIDAeroService.Entity;

namespace HIDAeroService.Mapper
{
    public class IntervalProfile : Profile
    {
        public IntervalProfile() 
        {
            CreateMap<ArInterval, IntervalDto>()
                .ForMember(dest => dest.Days, opt => opt.MapFrom(src => StringToDaysInWeek(src.Days)));
            CreateMap<IntervalDto, ArInterval>()
                .ForMember(dest => dest.IDays, opt => opt.MapFrom(src => (short)ConvertDayToBinary(src.Days)))
                .ForMember(dest => dest.Days, opt => opt.MapFrom(src => DaysInWeekToString(src.Days)))
                .ForMember(dest => dest.IStart, opt => opt.MapFrom(src => ConvertTimeToEndMinute(src.StartTime)))
                .ForMember(dest => dest.IEnd, opt => opt.MapFrom(src => ConvertTimeToEndMinute(src.Endtime)));
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
