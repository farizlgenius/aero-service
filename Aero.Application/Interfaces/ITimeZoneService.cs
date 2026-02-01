

using Aero.Application.DTOs;

namespace Aero.Application.Interface
{
    public interface ITimeZoneService
    {
        Task<ResponseDto<IEnumerable<TimeZoneDto>>> GetAsync();
        Task<ResponseDto<IEnumerable<TimeZoneDto>>> GetByLocationAsync(short location);
        Task<ResponseDto<TimeZoneDto>> GetByComponentIdAsync(short component);
        Task<ResponseDto<bool>> CreateAsync(TimeZoneDto dto);
        Task<ResponseDto<bool>> DeleteAsync(short component);
        Task<ResponseDto<TimeZoneDto>> UpdateAsync(TimeZoneDto dto);
        Task<ResponseDto<IEnumerable<Mode>>> GetModeAsync(int param);
        Task<ResponseDto<IEnumerable<Mode>>> GetCommandAsync();
    }
}
