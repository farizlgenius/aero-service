using AeroService.DTO;
using AeroService.DTO.CardFormat;

namespace AeroService.Service
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
