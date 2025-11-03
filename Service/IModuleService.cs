using Microsoft.AspNetCore.Mvc;
using HIDAeroService.DTO;
using HIDAeroService.DTO.Module;

namespace HIDAeroService.Service
{
    public interface IModuleService
    {
        Task<ResponseDto<IEnumerable<ModuleDto>>> GetAsync();
        void GetSioStatus(int ScpId, int SioNo);
        Task<ResponseDto<bool>> GetStatusAsync(string mac, short component);
        Task<ResponseDto<ModuleDto>> CreateAsync(ModuleDto dto);
        Task<ResponseDto<ModuleDto>> DeleteAsync(string mac, short component);
        Task<ResponseDto<ModuleDto>> UpdateAsync(ModuleDto dto);
        Task<ResponseDto<IEnumerable<ModeDto>>> GetModeAsync(int param);
        Task<ResponseDto<ModuleDto>> GetByComponentAsync(string mac, short component);
        Task<ResponseDto<IEnumerable<ModuleDto>>> GetByMacAsync(string mac);
        void TriggerDeviceStatus(int ScpId, short SioNo, string Status, string Tamper, string Ac, string Batt);
    }
}
