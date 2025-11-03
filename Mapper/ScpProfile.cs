using AutoMapper;
using HIDAeroService.DTO.Scp;
using HIDAeroService.Entity;

namespace HIDAeroService.Mapper
{
    public class ScpProfile : Profile
    {
        public ScpProfile() 
        {
            CreateMap<HardwareDto, Hardware>();
            CreateMap<Hardware, HardwareDto>();
            CreateMap<HardwareDto, Module>();
        }
    }
}
