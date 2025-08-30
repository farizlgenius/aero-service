using AutoMapper;
using HIDAeroService.Dto.CardFormat;
using HIDAeroService.Entity;

namespace HIDAeroService.Mapper
{
    public class CardFormatProfile : Profile
    {
        public CardFormatProfile()
        {
            CreateMap<ArCardFormat, CardFormatDto>();
            CreateMap<CardFormatDto, ArCardFormat>();

            // Create
            CreateMap<CreateCardFormatDto, ArCardFormat>();
        }

    }
}
