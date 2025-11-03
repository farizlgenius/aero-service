using AutoMapper;
using HIDAeroService.DTO.Holiday;
using HIDAeroService.Entity;

namespace HIDAeroService.Mapper
{
    public class HolidayProfile : Profile
    {
        public HolidayProfile()
        {
            CreateMap<Holiday, HolidayDto>();
            CreateMap<HolidayDto, Holiday>();
        }
    }
}
