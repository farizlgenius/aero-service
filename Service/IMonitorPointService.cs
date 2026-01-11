using AeroService.DTO;
using AeroService.DTO.MonitorPoint;
using Microsoft.AspNetCore.Mvc;

namespace AeroService.Service
{
    public interface IMonitorPointService
    {
        Task<ResponseDto<IEnumerable<MonitorPointDto>>> GetAsync(); 
            Task<ResponseDto<IEnumerable<MonitorPointDto>>> GetByLocationAsync(short location);

        Task<ResponseDto<IEnumerable<MonitorPointDto>>> GetByIdAndMacAsync(string mac);
        Task<ResponseDto<bool>> CreateAsync(MonitorPointDto dto);
        Task<ResponseDto<bool>> DeleteAsync(short component);
        Task<ResponseDto<IEnumerable<ResponseDto<bool>>>> DeleteRangeAsync(List<short> components);
        Task<ResponseDto<MonitorPointDto>> UpdateAsync(MonitorPointDto dto);
        Task<ResponseDto<bool>> GetStatusAsync(string mac, short component);
        Task<ResponseDto<IEnumerable<ModeDto>>> GetModeAsync(int param);
        Task<ResponseDto<MonitorPointDto>> GetByIdAndMacAsync(string mac, short component);
        Task<ResponseDto<IEnumerable<short>>> GetAvailableIp(string mac, short sio);
        Task<ResponseDto<bool>> MaskAsync(MonitorPointDto dto, bool IsMask);
        void TriggerDeviceStatus(int ScpId, short first, string status);
        Task<ResponseDto<IEnumerable<ModeDto>>> GetLogFunctionAsync();
    }
}
