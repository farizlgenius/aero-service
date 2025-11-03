using AutoMapper;
using HIDAeroService.DTO;
using HIDAeroService.DTO.Acr;
using HIDAeroService.Entity;

namespace HIDAeroService.Mapper
{
    public class AcrProfile : Profile
    {
        public AcrProfile() 
        {
            CreateMap<DoorDto, Door>();
            CreateMap<Door, DoorDto>();
            CreateMap<AntipassbackMode,ModeDto>();
            CreateMap<StrikeMode, ModeDto>();
            CreateMap<ReaderConfigurationMode, ModeDto>();
            CreateMap<DoorMode, ModeDto>();
            CreateMap<InputMode, ModeDto>();
            CreateMap<DoorDto, Door>();
            CreateMap<Door, DoorDto>();
        }
    }
}
