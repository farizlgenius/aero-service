
using Aero.Application.DTOs;
using Aero.Domain.Entities;

namespace Aero.Application.Interface
{
    public interface IMonitorPointService
    {
        Task<ResponseDto<IEnumerable<MonitorPointDto>>> GetAsync(); 

        Task<ResponseDto<MonitorPointDto>> GetByComponentIdAsync(short component);
            Task<ResponseDto<IEnumerable<MonitorPointDto>>> GetByLocationAsync(short location);

        Task<ResponseDto<IEnumerable<MonitorPointDto>>> GetByMacAsync(string mac);
        Task<ResponseDto<bool>> CreateAsync(MonitorPointDto dto);
        Task<ResponseDto<bool>> DeleteAsync(short component);
        Task<ResponseDto<IEnumerable<ResponseDto<bool>>>> DeleteRangeAsync(List<short> components);
        Task<ResponseDto<MonitorPointDto>> UpdateAsync(MonitorPointDto dto);
        Task<ResponseDto<bool>> GetStatusByComponentIdAsync(short component);
        Task<ResponseDto<IEnumerable<Mode>>> GetModeAsync(int param);
        Task<ResponseDto<IEnumerable<short>>> GetAvailableIp(string mac, short sio);
        Task<ResponseDto<bool>> MaskAsync(MonitorPointDto dto, bool IsMask);

        Task<ResponseDto<IEnumerable<Mode>>> GetLogFunctionAsync();
        Task<ResponseDto<Pagination<MonitorPointDto>>> GetPaginationAsync(PaginationParamsWithFilter param,short location);
    }
}
