

using Aero.Application.DTOs;
using Aero.Domain.Entities;

namespace Aero.Application.Interface
{
    public interface IIntervalService
    {
        Task<ResponseDto<IEnumerable<IntervalDto>>> GetAsync();
        Task<ResponseDto<IEnumerable<IntervalDto>>> GetByLocationAsync(short location);
        Task<ResponseDto<Pagination<IntervalDto>>> GetPaginationAsync(PaginationParamsWithFilter param,short location);
        Task<ResponseDto<IntervalDto>> GetByIdAsync(short component);
        Task<ResponseDto<bool>> CreateAsync(IntervalDto dto);
        Task<ResponseDto<IntervalDto>> UpdateAsync(IntervalDto dto);
        Task<ResponseDto<bool>> DeleteAsync(short component);
        Task<ResponseDto<IEnumerable<ResponseDto<bool>>>> DeleteRangeAsync(List<short> components);
    }
}
