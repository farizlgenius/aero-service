
using Aero.Application.DTOs;

namespace Aero.Application.Interface
{
    public interface IMonitorPointService
    {
        Task<ResponseDto<IEnumerable<MonitorPointDto>>> GetAsync(); 
            Task<ResponseDto<IEnumerable<MonitorPointDto>>> GetByLocationAsync(short location);

        Task<ResponseDto<IEnumerable<MonitorPointDto>>> GetByMacAsync(string mac);
        Task<ResponseDto<bool>> CreateAsync(MonitorPointDto dto);
        Task<ResponseDto<bool>> DeleteAsync(short component);
        Task<ResponseDto<IEnumerable<ResponseDto<bool>>>> DeleteRangeAsync(List<short> components);
        Task<ResponseDto<MonitorPointDto>> UpdateAsync(MonitorPointDto dto);
        Task<ResponseDto<bool>> GetStatusByComponentIdAsync(short component);
        Task<ResponseDto<IEnumerable<ModeDto>>> GetModeAsync(int param);
        Task<ResponseDto<IEnumerable<short>>> GetAvailableIp(string mac, short sio);
        Task<ResponseDto<bool>> MaskAsync(MonitorPointDto dto, bool IsMask);

        Task<ResponseDto<IEnumerable<ModeDto>>> GetLogFunctionAsync();
    }
}
