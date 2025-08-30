using HIDAeroService.Dto.CardFormat;
using HIDAeroService.Entity;

namespace HIDAeroService.Service.Interface
{
    public interface ICardFormatService
    {
        IEnumerable<ArCardFormat> GetAllSetting();
        Task<IEnumerable<CardFormatDto>> GetAll();
        Task<CardFormatDto> Add(CreateCardFormatDto dto);
        Task<bool> Save(CreateCardFormatDto dto,short cardFormatNo);
        Task<CardFormatDto> Delete(short cardFormatNo);
    }
}
