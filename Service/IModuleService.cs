using Microsoft.AspNetCore.Mvc;
using HIDAeroService.DTO;
using HIDAeroService.DTO.Module;
using HID.Aero.ScpdNet.Wrapper;

namespace HIDAeroService.Service
{
    public interface IModuleService
    {
        Task<ResponseDto<IEnumerable<ModuleDto>>> GetAsync();
        Task<ResponseDto<IEnumerable<ModuleDto>>> GetByLocationAsync(short location);
        void GetSioStatus(int ScpId, int SioNo);
        Task<ResponseDto<bool>> GetStatusAsync(string mac, short component);
        Task<ResponseDto<ModuleDto>> CreateAsync(ModuleDto dto);
        Task<ResponseDto<ModuleDto>> DeleteAsync(string mac, short component);
        Task<ResponseDto<ModuleDto>> UpdateAsync(ModuleDto dto);
        Task<ResponseDto<IEnumerable<ModeDto>>> GetModeAsync(int param);
        Task<ResponseDto<ModuleDto>> GetByComponentAsync(string mac, short component);
        Task<ResponseDto<IEnumerable<ModuleDto>>> GetByMacAsync(string mac);
        void TriggerDeviceStatus(int ScpId, short SioNo, string Status, string Tamper, string Ac, string Batt);
        Task<ResponseDto<IEnumerable<ModeDto>>> GetBaudrateAsync();
        Task<ResponseDto<IEnumerable<ModeDto>>> GetProtocolAsync();
        Task HandleFoundModuleAsync(SCPReplyMessage message);
    }
}
