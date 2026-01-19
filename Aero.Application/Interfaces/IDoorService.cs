

using Aero.Application.DTOs;

namespace Aero.Application.Interface
{
    public interface IDoorService
    {
        Task<ResponseDto<IEnumerable<DoorDto>>> GetAsync(); 
            Task<ResponseDto<IEnumerable<DoorDto>>> GetByLocationIdAsync(short location);
        Task<ResponseDto<bool>> CreateAsync(DoorDto dto);
        Task<ResponseDto<bool>> DeleteAsync(string mac, short component);
        Task<ResponseDto<DoorDto>> UpdateAsync(DoorDto dto);
        Task<ResponseDto<bool>> GetStatusAsync(string mac, short component);
        Task<ResponseDto<IEnumerable<DoorDto>>> GetByMacAsync(string mac);
        Task<ResponseDto<DoorDto>> GetByComponentAsync(string mac, short component);
        Task<ResponseDto<IEnumerable<ModeDto>>> GetModeAsync(int param);
        Task<ResponseDto<bool>> UnlockAsync(string mac, short component);
        Task<ResponseDto<IEnumerable<short>>> AvailableReaderAsync(string mac, short component);
        Task<ResponseDto<bool>> ChangeModeAsync(ChangeDoorModeDto dto);
        Task TriggerDeviceStatusAsync(int ScpId, short AcrNo, string AcrMode, string AccessPointStatus);
        void TriggerDeviceStatus(int ScpId, short AcrNo, string AcrMode, string AccessPointStatus);
        Task<ResponseDto<IEnumerable<ModeDto>>> GetOsdpBaudRate();
        Task<ResponseDto<IEnumerable<ModeDto>>> GetOsdpAddress();
        Task<ResponseDto<IEnumerable<ModeDto>>> GetAvailableOsdpAddress(string mac,short component);

    }
}
