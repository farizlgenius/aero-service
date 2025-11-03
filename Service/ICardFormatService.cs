using HIDAeroService.DTO;
using HIDAeroService.DTO.CardFormat;

namespace HIDAeroService.Service
{
    public interface ICardFormatService
    {
        Task<ResponseDto<IEnumerable<CardFormatDto>>> GetAsync();
        Task<ResponseDto<CardFormatDto>> GetByComponentIdAsync(short component);
        Task<ResponseDto<bool>> CreateAsync(CardFormatDto dto);
        Task<ResponseDto<bool>> DeleteAsync(short component);
        Task<ResponseDto<CardFormatDto>> UpdateAsync(CardFormatDto dto);
    }
}
