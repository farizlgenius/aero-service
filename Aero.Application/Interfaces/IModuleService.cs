
using Aero.Application.DTOs;
using Aero.Domain.Entities;

namespace Aero.Application.Interface
{
    public interface IModuleService
    {
        Task<ResponseDto<IEnumerable<ModuleDto>>> GetAsync();
        Task<ResponseDto<IEnumerable<ModuleDto>>> GetByLocationAsync(int location);
        Task<ResponseDto<Pagination<ModuleDto>>> GetPaginationAsync(PaginationParamsWithFilter param,int location);
        void GetSioStatus(int ScpId, int SioNo);
        Task<ResponseDto<bool>> GetStatusAsync(int device, short driver);
        Task<ResponseDto<ModuleDto>> CreateAsync(ModuleDto dto);
        Task<ResponseDto<ModuleDto>> DeleteAsync(string mac, short component);
        Task<ResponseDto<ModuleDto>> UpdateAsync(ModuleDto dto);
        Task<ResponseDto<IEnumerable<ModeDto>>> GetModeAsync(int param);
        Task<ResponseDto<ModuleDto>> GetByComponentAsync(string mac, short component);
        Task<ResponseDto<IEnumerable<ModuleDto>>> GetByDeviceIdAsync(int device);
        Task<ResponseDto<IEnumerable<ModeDto>>> GetBaudrateAsync();
        Task<ResponseDto<IEnumerable<ModeDto>>> GetProtocolAsync();

    }
}
