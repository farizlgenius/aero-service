

using Aero.Application.DTOs;
using Aero.Domain.Entities;

namespace Aero.Application.Interface
{
    public interface ITimeZoneService
    {
        Task<ResponseDto<IEnumerable<TimeZoneDto>>> GetAsync();
        Task<ResponseDto<Pagination<TimeZoneDto>>> GetPaginationAsync(PaginationParamsWithFilter param,short location);
        Task<ResponseDto<IEnumerable<TimeZoneDto>>> GetByLocationAsync(short location);
        Task<ResponseDto<TimeZoneDto>> GetByComponentIdAsync(short component);
        Task<ResponseDto<bool>> CreateAsync(TimeZoneDto dto);
        Task<ResponseDto<bool>> DeleteAsync(short component);
        Task<ResponseDto<TimeZoneDto>> UpdateAsync(TimeZoneDto dto);
        Task<ResponseDto<IEnumerable<Mode>>> GetModeAsync(int param);
        Task<ResponseDto<IEnumerable<Mode>>> GetCommandAsync();
        Task<ResponseDto<IEnumerable<ResponseDto<bool>>>> DeleteRangeAsync(List<short> components);
    }
}
