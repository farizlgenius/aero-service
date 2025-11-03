using HIDAeroService.DTO;
using HIDAeroService.DTO.ControlPoint;
using HIDAeroService.DTO.Output;

namespace HIDAeroService.Service
{
    public interface IControlPointService
    {
        Task<ResponseDto<IEnumerable<ControlPointDto>>> GetAsync();
        Task<ResponseDto<bool>> CreateOutputAsync(ControlPointDto dto);
        Task<ResponseDto<bool>> DeleteAsync(string mac, short component);
        Task<ResponseDto<ControlPointDto>> UpdateAsync(ControlPointDto dto);
        Task<ResponseDto<bool>> GetStatusAsync(string mac, short component);
        Task<ResponseDto<IEnumerable<ModeDto>>> GetModeAsync(int param);
        Task<ResponseDto<ControlPointDto>> GetByMacAndIdAsync(string mac, short component);
        Task<ResponseDto<bool>> ToggleAsync(ToggleControlPointDto cpTriggerDto);
        Task<ResponseDto<IEnumerable<short>>> GetAvailableOpAsync(string mac, short component);
        void TriggerDeviceStatus(string ScpMac, int first, string status);

    }
}
