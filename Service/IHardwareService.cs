using AeroService.DTO.Hardware;
using HID.Aero.ScpdNet.Wrapper;
using AeroService.Aero.CommandService;
using AeroService.Aero.CommandService.Impl;
using AeroService.Data;
using AeroService.DTO;
using AeroService.DTO.Hardware;
using AeroService.DTO.IdReport;
using AeroService.DTO.Scp;
using AeroService.Entity;
using LibNoise.Combiner;
using Microsoft.AspNetCore.Mvc;

namespace AeroService.Service
{
    public interface IHardwareService
    {
        
        Task<ResponseDto<IEnumerable<HardwareDto>>> GetAsync();
        Task<ResponseDto<IEnumerable<ModeDto>>> GetHardwareTypeAsync();
        Task<ResponseDto<IEnumerable<HardwareDto>>> GetByLocationAsync(short location);
        Task<ResponseDto<bool>> CreateAsync(CreateHardwareDto dto);
      Task<ResponseDto<bool>> DeleteAsync(string mac);
       Task<ResponseDto<HardwareDto>> UpdateAsync(HardwareDto dto);

      Task<ResponseDto<IEnumerable<ModeDto>>> GetModeAsync(int param);

       Task<ResponseDto<HardwareDto>> GetByMacAsync(string mac);
        Task<ResponseDto<bool>> ResetAsync(string mac);
        Task<ResponseDto<bool>> ResetAsync(short id);
        Task<ResponseDto<bool>> UploadComponentConfigurationAsync(string mac);
        Task<ResponseDto<bool>> VerifyMemoryAllocateAsyncWithResponse(string mac);
        Task<bool> VerifyMemoryAllocateAsync(string mac);
        Task<ResponseDto<IEnumerable<VerifyHardwareDeviceConfigDto>>> VerifyComponentConfigurationAsync(string mac);
        Task<ResponseDto<HardwareStatus>> GetStatusAsync(string mac);
        void TriggerDeviceStatus(string ScpMac, int CommStatus);
        void TriggerIdReport(List<IdReportDto> IdReports);
        void TriggerTranStatus(SCPReplyMessage message);
        Task VerifyAllocateHardwareMemoryAsync(SCPReplyMessage message);
        Task AssignPort(SCPReplyMessage message);
        Task AssignIpAddress(SCPReplyMessage message);
        Task HandleFoundHardware(SCPReplyMessage message);
        Task<ResponseDto<bool>> GetTransactionLogStatusAsync(string mac);
        Task<IEnumerable<IdReportDto>> RemoveIdReportAsync(SCPReplyMessage message);
        Task<ResponseDto<bool>> SetTransactionAsync(string mac, short IsOn);
        Task<ResponseDto<IEnumerable<ResponseDto<bool>>>> SetRangeTransactionAsync(List<SetTranDto> tran);

    }
}
