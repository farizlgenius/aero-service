using HIDAeroService.DTO;
using HIDAeroService.DTO.MonitorGroup;
using Microsoft.AspNetCore.Mvc;

namespace HIDAeroService.Service
{
    public interface IMonitorGroupService
    {
        Task<ResponseDto<IEnumerable<MonitorGroupDto>>> GetAsync();
        Task<ResponseDto<MonitorGroupDto>> CreateAsync(MonitorGroupDto dto);
        Task<ResponseDto<MonitorGroupDto>> DeleteAsync(string mac,short component);
        Task<ResponseDto<MonitorGroupDto>> UpdateAsync(MonitorGroupDto dto);
    }
}
