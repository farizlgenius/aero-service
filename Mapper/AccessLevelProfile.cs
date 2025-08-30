using AutoMapper;
using HIDAeroService.Dto.AccessLevel;
using HIDAeroService.Dto.CardFormat;
using HIDAeroService.Entity;

namespace HIDAeroService.Mapper
{
    public class AccessLevelProfile : Profile
    {
        public AccessLevelProfile()
        {
            CreateMap<ArAccessLevel, AccessLevelDto>();
            CreateMap<AccessLevelDto, ArAccessLevel>();

            // TimeZone
            CreateMap<ArAccessLevel, AccessLevelTimeZoneDto>();

            // Create 
            CreateMap<CreateAccessLevelDto,ArAccessLevel>();
        }
    }
}
