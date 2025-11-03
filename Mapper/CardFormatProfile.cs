using AutoMapper;
using HIDAeroService.DTO.CardFormat;
using HIDAeroService.Entity;

namespace HIDAeroService.Mapper
{
    public class CardFormatProfile : Profile
    {
        public CardFormatProfile()
        {
            CreateMap<CardFormat, CardFormatDto>();
            CreateMap<CardFormatDto, CardFormat>();

        }

    }
}
