
using Aero.Application.DTOs;
using Aero.Domain.Entities;

namespace Aero.Application.Interface
{
    public interface IMonitorPointService
    {
        Task<ResponseDto<IEnumerable<MonitorPointDto>>> GetAsync(); 

        Task<ResponseDto<MonitorPointDto>> GetByIdAsync(int id);
            Task<ResponseDto<IEnumerable<MonitorPointDto>>> GetByLocationAsync(int location);

        Task<ResponseDto<IEnumerable<MonitorPointDto>>> GetByDviceIdAsync(int id);
        Task<ResponseDto<MonitorPointDto>> CreateAsync(MonitorPointDto dto);
        Task<ResponseDto<MonitorPointDto>> DeleteAsync(int id);
        Task<ResponseDto<IEnumerable<MonitorPointDto>>> DeleteRangeAsync(List<int> ids);
        Task<ResponseDto<MonitorPointDto>> UpdateAsync(MonitorPointDto dto);
        Task<ResponseDto<bool>> GetStatusByIdAsync(int id);
        Task<ResponseDto<IEnumerable<ModeDto>>> GetModeAsync(int param);
        Task<ResponseDto<IEnumerable<short>>> GetAvailableIp(int id);
        Task<ResponseDto<bool>> MaskAsync(MonitorPointDto dto, bool IsMask);

        Task<ResponseDto<IEnumerable<ModeDto>>> GetLogFunctionAsync();
        Task<ResponseDto<Pagination<MonitorPointDto>>> GetPaginationAsync(PaginationParamsWithFilter param,int location);
    }
}
