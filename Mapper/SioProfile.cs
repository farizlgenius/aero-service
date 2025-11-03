using AutoMapper;
using HIDAeroService.DTO.Module;
using HIDAeroService.Entity;

namespace HIDAeroService.Mapper
{
    public class SioProfile : Profile
    {
        public SioProfile() 
        {
            CreateMap<Module, ModuleDto>();
            CreateMap<ModuleDto, Module>();
        }
    }
}
