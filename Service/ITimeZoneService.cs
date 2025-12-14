using HIDAeroService.DTO;
using HIDAeroService.DTO.TimeZone;

namespace HIDAeroService.Service
{
    public interface ITimeZoneService
    {
        Task<ResponseDto<IEnumerable<TimeZoneDto>>> GetAsync();
        Task<ResponseDto<TimeZoneDto>> GetByComponentIdAsync(short component);
        Task<ResponseDto<bool>> CreateAsync(CreateTimeZoneDto dto);
        Task<ResponseDto<bool>> DeleteAsync(short component);
        Task<ResponseDto<TimeZoneDto>> UpdateAsync(TimeZoneDto dto);
        Task<ResponseDto<IEnumerable<ModeDto>>> GetModeAsync(int param);
        Task<ResponseDto<IEnumerable<ModeDto>>> GetCommandAsync();
    }
}
