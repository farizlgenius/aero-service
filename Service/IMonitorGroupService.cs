using AeroService.DTO;
using AeroService.DTO.MonitorGroup;
using Microsoft.AspNetCore.Mvc;

namespace AeroService.Service
{
    public interface IMonitorGroupService
    {
        Task<ResponseDto<IEnumerable<MonitorGroupDto>>> GetAsync();
        Task<ResponseDto<IEnumerable<ModeDto>>> GetCommandAsync();
        Task<ResponseDto<IEnumerable<ModeDto>>> GetTypeAsync();
        Task<ResponseDto<IEnumerable<MonitorGroupDto>>> GetByLocationAsync(short location);
        Task<ResponseDto<bool>> MonitorGroupCommandAsync(MonitorGroupCommandDto dto);
        Task<ResponseDto<bool>> CreateAsync(MonitorGroupDto dto);
        Task<ResponseDto<bool>> DeleteAsync(string mac,short component);
        Task<ResponseDto<MonitorGroupDto>> UpdateAsync(MonitorGroupDto dto);
    }
}
