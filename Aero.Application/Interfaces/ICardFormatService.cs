

using Aero.Application.DTOs;
using Aero.Domain.Entities;

namespace Aero.Application.Interface
{
    public interface ICardFormatService
    {
        Task<ResponseDto<IEnumerable<CardFormatDto>>> GetAsync();
        Task<ResponseDto<CardFormatDto>> GetByComponentIdAsync(short component);
        Task<ResponseDto<IEnumerable<CardFormatDto>>> GetByLocationIdAsync(short location);
        Task<ResponseDto<Pagination<CardFormatDto>>> GetPaginationAsync(PaginationParamsWithFilter param,short location);
        Task<ResponseDto<bool>> CreateAsync(CardFormatDto dto);
        Task<ResponseDto<bool>> DeleteAsync(short component);
        Task<ResponseDto<CardFormatDto>> UpdateAsync(CardFormatDto dto);
    }
}
