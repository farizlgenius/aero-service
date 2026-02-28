

using Aero.Application.DTOs;
using Aero.Domain.Entities;

namespace Aero.Application.Interface
{
    public interface IIntervalService
    {
        Task<ResponseDto<IEnumerable<IntervalDto>>> GetAsync();
        Task<ResponseDto<IEnumerable<IntervalDto>>> GetByLocationAsync(int location);
        Task<ResponseDto<Pagination<IntervalDto>>> GetPaginationAsync(PaginationParamsWithFilter param,int location);
        Task<ResponseDto<IntervalDto>> GetByIdAsync(int compidonent);
        Task<ResponseDto<IntervalDto>> CreateAsync(CreateIntervalDto dto);
        Task<ResponseDto<IntervalDto>> UpdateAsync(IntervalDto dto);
        Task<ResponseDto<IntervalDto>> DeleteAsync(int id);
        Task<ResponseDto<IEnumerable<IntervalDto>>> DeleteRangeAsync(List<int> ids);
    }
}
