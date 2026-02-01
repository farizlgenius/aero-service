
using Aero.Application.DTOs;
using Aero.Domain.Entities;

namespace Aero.Application.Interface
{
    public interface IAccessAreaService
    {
        Task<ResponseDto<IEnumerable<AccessAreaDto>>> GetAsync();
        Task<ResponseDto<AccessAreaDto>> GetByComponentAsync(short component);
        Task<ResponseDto<bool>> CreateAsync(AccessAreaDto dto);
        Task<ResponseDto<bool>> DeleteAsync(short component);
        Task<ResponseDto<AccessAreaDto>> UpdateAsync(AccessAreaDto dto);
        Task<ResponseDto<IEnumerable<Mode>>> GetCommandAsync();
        Task<ResponseDto<IEnumerable<Mode>>> GetAccessControlOptionAsync();
        Task<ResponseDto<IEnumerable<Mode>>> GetOccupancyControlOptionAsync();
        Task<ResponseDto<IEnumerable<Mode>>> GetAreaFlagOptionAsync();
        Task<ResponseDto<IEnumerable<Mode>>> GetMultiOccupancyOptionAsync();
    }
}
