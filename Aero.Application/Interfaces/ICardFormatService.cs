

using Aero.Application.DTOs;
using Aero.Domain.Entities;

namespace Aero.Application.Interface
{
    public interface ICardFormatService
    {
        Task<ResponseDto<IEnumerable<CardFormatDto>>> GetAsync();
        Task<ResponseDto<CardFormatDto>> GetByIdAsync(int id);
        Task<ResponseDto<IEnumerable<CardFormatDto>>> GetByLocationIdAsync(int location);
        Task<ResponseDto<Pagination<CardFormatDto>>> GetPaginationAsync(PaginationParamsWithFilter param,int location);
        Task<ResponseDto<CardFormatDto>> CreateAsync(CardFormatDto dto);
        Task<ResponseDto<CardFormatDto>> DeleteAsync(int id);
        Task<ResponseDto<CardFormatDto>> UpdateAsync(CardFormatDto dto);
    }
}
