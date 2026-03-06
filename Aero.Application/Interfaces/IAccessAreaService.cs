
using Aero.Application.DTOs;
using Aero.Domain.Entities;

namespace Aero.Application.Interface
{
    public interface IAccessAreaService
    {
        Task<ResponseDto<IEnumerable<AccessAreaDto>>> GetAsync();
        Task<ResponseDto<AccessAreaDto>> GetByComponentAsync(int id);
        Task<ResponseDto<IEnumerable<AccessAreaDto>>> GetByLocationIdAsync(int location);
        Task<ResponseDto<Pagination<AccessAreaDto>>> GetPaginationAsync(PaginationParamsWithFilter param, int location);
        Task<ResponseDto<AccessAreaDto>> CreateAsync(CreateAccessAreaDto dto);
        Task<ResponseDto<AccessAreaDto>> DeleteAsync(int id);
        Task<ResponseDto<AccessAreaDto>> UpdateAsync(AccessAreaDto dto);
        Task<ResponseDto<IEnumerable<ModeDto>>> GetCommandAsync();
        Task<ResponseDto<IEnumerable<ModeDto>>> GetAccessControlOptionAsync();
        Task<ResponseDto<IEnumerable<ModeDto>>> GetOccupancyControlOptionAsync();
        Task<ResponseDto<IEnumerable<ModeDto>>> GetAreaFlagOptionAsync();
        Task<ResponseDto<IEnumerable<ModeDto>>> GetMultiOccupancyOptionAsync();
    }
}
