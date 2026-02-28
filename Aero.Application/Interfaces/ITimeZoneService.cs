

using Aero.Application.DTOs;
using Aero.Domain.Entities;

namespace Aero.Application.Interface
{
    public interface ITimeZoneService
    {
        Task<ResponseDto<IEnumerable<TimeZoneDto>>> GetAsync();
        Task<ResponseDto<Pagination<TimeZoneDto>>> GetPaginationAsync(PaginationParamsWithFilter param,int id);
        Task<ResponseDto<IEnumerable<TimeZoneDto>>> GetByLocationAsync(short location);
        Task<ResponseDto<TimeZoneDto>> GetByIdAsync(int id);
        Task<ResponseDto<TimeZoneDto>> CreateAsync(CreateTimeZoneDto dto);
        Task<ResponseDto<TimeZoneDto>> DeleteByIdAsync(int id);
        Task<ResponseDto<TimeZoneDto>> UpdateAsync(TimeZoneDto dto);
        Task<ResponseDto<IEnumerable<Mode>>> GetModeAsync(int param);
        Task<ResponseDto<IEnumerable<Mode>>> GetCommandAsync();
        Task<ResponseDto<IEnumerable<TimeZoneDto>>> DeleteRangeAsync(List<short> components);
    }
}
