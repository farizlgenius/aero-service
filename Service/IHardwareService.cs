using HID.Aero.ScpdNet.Wrapper;
using HIDAeroService.AeroLibrary;
using HIDAeroService.DTO;
using HIDAeroService.DTO.Hardware;
using HIDAeroService.DTO.Scp;
using HIDAeroService.Entity;
using HIDAeroService.Model;
using LibNoise.Combiner;
using Microsoft.AspNetCore.Mvc;

namespace HIDAeroService.Service
{
    public interface IHardwareService
    {
        Task<ResponseDto<bool>> SetTransactionAsync(string mac, short IsOn);
        Task<ResponseDto<IEnumerable<HardwareDto>>> GetAsync();
        Task<ResponseDto<bool>> CreateAsync(CreateHardwareDto dto);
      Task<ResponseDto<HardwareDto>> DeleteAsync(string mac);
       Task<ResponseDto<HardwareDto>> UpdateAsync(HardwareDto dto);

      Task<ResponseDto<IEnumerable<ModeDto>>> GetModeAsync(int param);

       Task<ResponseDto<HardwareDto>> GetByMacAsync(string mac);
        Task<ResponseDto<bool>> ResetAsync(string mac);
        Task<ResponseDto<bool>> ResetAsync(short id);
        Task<ResponseDto<bool>> UploadConfigAsync(string mac);
        Task<bool> VerifySystemConfigurationAsync(short id);
        Task<ResponseDto<HardwareStatus>> GetStatusAsync(string mac,short id);
        void TriggerDeviceStatus(string ScpMac, int CommStatus);
        void TriggerIdReport(List<IdReport> IdReports);
        Task VerifyAllocateHardwareMemoryAsync(SCPReplyMessage message);
        void HandleUploadCommand(AeroCommand command, SCPReplyMessage message);
        void AssignIpToIdReport(SCPReplyMessage message, List<IdReport> iDReports);
        Task<IdReport> HandleFoundHardware(SCPReplyMessage message, ITimeZoneService tz, ICardFormatService cfmt, IAccessLevelService alvl);

    }
}
