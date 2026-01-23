

using Aero.Application.DTOs;
using Aero.Domain.Entities;

namespace Aero.Application.Interface
{
    public interface IHardwareService
    {

        Task<ResponseDto<IEnumerable<HardwareDto>>> GetAsync();
        Task<ResponseDto<IEnumerable<ModeDto>>> GetHardwareTypeAsync();
        Task<ResponseDto<IEnumerable<HardwareDto>>> GetByLocationAsync(short location);
        Task<ResponseDto<bool>> CreateAsync(CreateHardwareDto dto);
        Task<ResponseDto<bool>> DeleteAsync(string mac);
        Task<ResponseDto<HardwareDto>> UpdateAsync(HardwareDto dto);

        Task<ResponseDto<HardwareDto>> GetByMacAsync(string mac);
        Task<ResponseDto<bool>> ResetByMacAsync(string mac);
        Task<ResponseDto<bool>> ResetByComponentAsync(short id);
        Task<ResponseDto<bool>> UploadComponentConfigurationAsync(string mac);
        Task<ResponseDto<bool>> VerifyMemoryAllocateAsyncWithResponse(string mac);
        Task<bool> VerifyMemoryAllocateAsync(string mac);
        Task<ResponseDto<IEnumerable<VerifyHardwareDeviceConfigDto>>> VerifyComponentConfigurationAsync(string mac);
        Task<ResponseDto<HardwareStatus>> GetStatusAsync(string mac);
        Task<ResponseDto<bool>> GetTransactionLogStatusAsync(string mac);
        Task<ResponseDto<bool>> SetTransactionAsync(string mac, short IsOn);
        Task<ResponseDto<IEnumerable<ResponseDto<bool>>>> SetRangeTransactionAsync(List<SetTranDto> tran);
        Task<bool> MappingHardwareAndAllocateMemory(short ScpId);
        Task<List<VerifyHardwareDeviceConfigDto>> VerifyDeviceConfigurationAsync(Hardware hw);
        Task<bool> VerifyHardwareConnection(short ScpId);

    }
}
