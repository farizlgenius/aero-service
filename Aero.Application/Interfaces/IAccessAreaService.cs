
using Aero.Application.DTOs;

namespace Aero.Application.Interface
{
    public interface IAccessAreaService
    {
        Task<ResponseDto<IEnumerable<AccessAreaDto>>> GetAsync();
        Task<ResponseDto<AccessAreaDto>> GetByComponentAsync(short component);
        Task<ResponseDto<bool>> CreateAsync(AccessAreaDto dto);
        Task<ResponseDto<bool>> DeleteAsync(short component);
        Task<ResponseDto<AccessAreaDto>> UpdateAsync(AccessAreaDto dto);
        Task<ResponseDto<IEnumerable<ModeDto>>> GetCommandAsync();
        Task<ResponseDto<IEnumerable<ModeDto>>> GetAccessControlOptionAsync();
        Task<ResponseDto<IEnumerable<ModeDto>>> GetOccupancyControlOptionAsync();
        Task<ResponseDto<IEnumerable<ModeDto>>> GetAreaFlagOptionAsync();
        Task<ResponseDto<IEnumerable<ModeDto>>> GetMultiOccupancyOptionAsync();
    }
}
