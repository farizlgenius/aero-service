

using Aero.Application.DTOs;

namespace Aero.Application.Interface
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
