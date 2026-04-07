

using Aero.Application.DTOs;
using Aero.Domain.Entities;

namespace Aero.Application.Interface
{
    public interface IMonitorGroupService
    {
        Task<ResponseDto<IEnumerable<MonitorGroupDto>>> GetAsync();
        Task<ResponseDto<Pagination<MonitorGroupDto>>> GetPaginationAsync(PaginationParamsWithFilter param,short location);
        Task<ResponseDto<IEnumerable<ModeDto>>> GetCommandAsync();
        Task<ResponseDto<IEnumerable<ModeDto>>> GetTypeAsync();
        Task<ResponseDto<IEnumerable<MonitorGroupDto>>> GetByLocationAsync(short location);
        Task<ResponseDto<bool>> MonitorGroupCommandAsync(MonitorGroupCommandDto dto);
        Task<ResponseDto<MonitorGroupDto>> CreateAsync(MonitorGroupDto dto);
        Task<ResponseDto<MonitorGroupDto>> DeleteAsync(int id);
        Task<ResponseDto<MonitorGroupDto>> UpdateAsync(MonitorGroupDto dto);
    }
}
