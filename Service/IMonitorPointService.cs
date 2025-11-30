using HIDAeroService.DTO;
using HIDAeroService.DTO.MonitorPoint;

namespace HIDAeroService.Service
{
    public interface IMonitorPointService
    {
        Task<ResponseDto<IEnumerable<MonitorPointDto>>> GetAsync(); 
            Task<ResponseDto<IEnumerable<MonitorPointDto>>> GetByLocationAsync(short location);
        Task<ResponseDto<bool>> CreateAsync(MonitorPointDto dto);
        Task<ResponseDto<bool>> DeleteAsync(string mac, short component);
        Task<ResponseDto<MonitorPointDto>> UpdateAsync(MonitorPointDto dto);
        Task<ResponseDto<bool>> GetStatusAsync(string mac, short component);
        Task<ResponseDto<IEnumerable<ModeDto>>> GetModeAsync(int param);
        Task<ResponseDto<MonitorPointDto>> GetByIdAndMacAsync(string mac, short component);
        Task<ResponseDto<IEnumerable<short>>> GetAvailableIp(string mac, short sio);
        Task<ResponseDto<bool>> MaskAsync(MonitorPointDto dto, bool IsMask);
        void TriggerDeviceStatus(int ScpId, short first, string status);
    }
}
