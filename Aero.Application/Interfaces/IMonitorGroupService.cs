

using Aero.Application.DTOs;

namespace Aero.Application.Interface
{
    public interface IMonitorGroupService
    {
        Task<ResponseDto<IEnumerable<MonitorGroupDto>>> GetAsync();
        Task<ResponseDto<IEnumerable<Mode>>> GetCommandAsync();
        Task<ResponseDto<IEnumerable<Mode>>> GetTypeAsync();
        Task<ResponseDto<IEnumerable<MonitorGroupDto>>> GetByLocationAsync(short location);
        Task<ResponseDto<bool>> MonitorGroupCommandAsync(MonitorGroupCommandDto dto);
        Task<ResponseDto<bool>> CreateAsync(MonitorGroupDto dto);
        Task<ResponseDto<bool>> DeleteAsync(string mac,short component);
        Task<ResponseDto<MonitorGroupDto>> UpdateAsync(MonitorGroupDto dto);
    }
}
