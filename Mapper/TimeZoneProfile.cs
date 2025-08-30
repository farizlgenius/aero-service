using AutoMapper;
using HIDAeroService.Dto.TimeZone;
using HIDAeroService.Entity;

namespace HIDAeroService.Mapper
{
    public class TimeZoneProfile : Profile
    {
        public TimeZoneProfile() 
        {
            CreateMap<ArTimeZone, TimeZoneDto>();
            CreateMap<TimeZoneDto, ArTimeZone>();
        }
    }
}
