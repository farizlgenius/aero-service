

using Aero.Application.DTOs;

namespace Aero.Application.Interface
{
    public interface IControlPointService
    {
        Task<ResponseDto<IEnumerable<ControlPointDto>>> GetAsync();
        Task<ResponseDto<IEnumerable<ControlPointDto>>> GetByLocationAsync(short location);
        
        Task<ResponseDto<bool>> CreateAsync(ControlPointDto dto);
        Task<ResponseDto<bool>> DeleteAsync(short component);
        Task<ResponseDto<ControlPointDto>> UpdateAsync(ControlPointDto dto);
        Task<ResponseDto<bool>> GetStatusAsync(string mac, short component);
        Task<ResponseDto<IEnumerable<ModeDto>>> GetModeAsync(int param);
        Task<ResponseDto<ControlPointDto>> GetByMacAndIdAsync(string mac, short component);
        Task<ResponseDto<bool>> ToggleAsync(ToggleControlPointDto cpTriggerDto);
        Task<ResponseDto<IEnumerable<short>>> GetAvailableOpAsync(string mac, short component);
        Task<ResponseDto<IEnumerable<ResponseDto<bool>>>> DeleteRangeAsync(List<short> components);

    }
}
