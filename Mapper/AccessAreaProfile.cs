using AutoMapper;
using HIDAeroService.DTO.AccessArea;
using HIDAeroService.Entity;

namespace HIDAeroService.Mapper
{
    public class AccessAreaProfile : Profile
    {
        public AccessAreaProfile() 
        {
            CreateMap<AccessArea, AccessAreaDto>();
            CreateMap<AccessAreaDto, AccessArea>();
        }
    }
}
