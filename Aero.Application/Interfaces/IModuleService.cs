
using Aero.Application.DTOs;
using Aero.Domain.Entities;

namespace Aero.Application.Interface
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
        Task<ResponseDto<IEnumerable<Mode>>> GetModeAsync(int param);
        Task<ResponseDto<ModuleDto>> GetByComponentAsync(string mac, short component);
        Task<ResponseDto<IEnumerable<ModuleDto>>> GetByMacAsync(string mac);
        Task<ResponseDto<IEnumerable<Mode>>> GetBaudrateAsync();
        Task<ResponseDto<IEnumerable<Mode>>> GetProtocolAsync();

    }
}
