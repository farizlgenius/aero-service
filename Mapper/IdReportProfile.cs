using AutoMapper;
using HIDAeroService.DTO.IdReport;
using HIDAeroService.Model;

namespace HIDAeroService.Mapper
{
    public class IdReportProfile : Profile
    {
        public IdReportProfile() 
        {
            CreateMap<IDReportDto,IdReport>();
            CreateMap<IdReport, IDReportDto>()
                .ForMember(dest => dest.Model, opt => opt.MapFrom(src => src.DeviceId == 3 ? "HID Aero" : ""));
        }
    }
}
