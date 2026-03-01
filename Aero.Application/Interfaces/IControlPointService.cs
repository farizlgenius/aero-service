

using Aero.Application.DTOs;
using Aero.Domain.Entities;

namespace Aero.Application.Interface
{
    public interface IControlPointService
    {
        Task<ResponseDto<IEnumerable<ControlPointDto>>> GetAsync();
        Task<ResponseDto<IEnumerable<ControlPointDto>>> GetByLocationAsync(int location);
        Task<ResponseDto<Pagination<ControlPointDto>>> GetPaginationAsync(PaginationParamsWithFilter param, int location);
        
        Task<ResponseDto<ControlPointDto>> CreateAsync(CreateControlPointDto dto);
        Task<ResponseDto<ControlPointDto>> DeleteAsync(int id);
        Task<ResponseDto<ControlPointDto>> UpdateAsync(ControlPointDto dto);
        Task<ResponseDto<bool>> GetStatusAsync(int deviceId, int driverId);
        Task<ResponseDto<IEnumerable<ModeDto>>> GetModeAsync(int param);
        Task<ResponseDto<ControlPointDto>> GetByDeviceAndIdAsync(int device, int id);
        Task<ResponseDto<bool>> ToggleAsync(ToggleControlPointDto cpTriggerDto);
        Task<ResponseDto<IEnumerable<short>>> GetAvailableOpAsync(int deviceId, int id);
        Task<ResponseDto<IEnumerable<ControlPointDto>>> DeleteRangeAsync(List<int> ids);

    }
}
