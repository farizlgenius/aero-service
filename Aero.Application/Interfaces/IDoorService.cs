

using Aero.Application.DTOs;
using Aero.Domain.Entities;

namespace Aero.Application.Interface
{
    public interface IDoorService
    {
        Task<ResponseDto<IEnumerable<DoorDto>>> GetAsync(); 
            Task<ResponseDto<IEnumerable<DoorDto>>> GetByLocationIdAsync(short location);
        Task<ResponseDto<Pagination<DoorDto>>> GetPaginationAsync(PaginationParamsWithFilter param,short location);
        Task<ResponseDto<bool>> CreateAsync(DoorDto dto);
        Task<ResponseDto<bool>> DeleteAsync(short component);
        Task<ResponseDto<DoorDto>> UpdateAsync(DoorDto dto);
        Task<ResponseDto<bool>> GetStatusAsync(string mac, short component);
        Task<ResponseDto<IEnumerable<DoorDto>>> GetByMacAsync(string mac);
        Task<ResponseDto<DoorDto>> GetByComponentAsync( short component);
        Task<ResponseDto<IEnumerable<Mode>>> GetModeAsync(int param);
        Task<ResponseDto<bool>> UnlockAsync(string mac, short component);
        Task<ResponseDto<IEnumerable<short>>> AvailableReaderAsync(string mac, short component);
        Task<ResponseDto<bool>> ChangeModeAsync(ChangeDoorModeDto dto);
        Task<ResponseDto<IEnumerable<Mode>>> GetOsdpBaudRate();
        Task<ResponseDto<IEnumerable<Mode>>> GetOsdpAddress();
        Task<ResponseDto<IEnumerable<Mode>>> GetAvailableOsdpAddress(string mac,short component);

    }
}
