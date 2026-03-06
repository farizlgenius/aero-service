

using Aero.Application.DTOs;
using Aero.Domain.Entities;

namespace Aero.Application.Interface
{
    public interface IDoorService
    {
        Task<ResponseDto<IEnumerable<DoorDto>>> GetAsync(); 
            Task<ResponseDto<IEnumerable<DoorDto>>> GetByLocationIdAsync(short location);
        Task<ResponseDto<Pagination<DoorDto>>> GetPaginationAsync(PaginationParamsWithFilter param,short location);
        Task<ResponseDto<DoorDto>> CreateAsync(CreateDoorDto dto);
        Task<ResponseDto<DoorDto>> DeleteAsync(int id);
        Task<ResponseDto<DoorDto>> UpdateAsync(DoorDto dto);
        Task<ResponseDto<bool>> GetStatusAsync(string mac, short component);
        Task<ResponseDto<IEnumerable<DoorDto>>> GetByDeviceIdAsync(int id);
        Task<ResponseDto<DoorDto>> GetByComponentAsync( short component);
        Task<ResponseDto<IEnumerable<ModeDto>>> GetModeAsync(int param);
        Task<ResponseDto<bool>> UnlockAsync(string mac, short component);
        Task<ResponseDto<IEnumerable<short>>> AvailableReaderAsync(int device, int driver);
        Task<ResponseDto<bool>> ChangeModeAsync(ChangeDoorModeDto dto);
        Task<ResponseDto<IEnumerable<ModeDto>>> GetOsdpBaudRate();
        Task<ResponseDto<IEnumerable<ModeDto>>> GetOsdpAddress();
        Task<ResponseDto<IEnumerable<ModeDto>>> GetAvailableOsdpAddress(string mac,short component);

    }
}
