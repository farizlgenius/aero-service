
using Aero.Application.DTOs;
using Aero.Domain.Entities;

namespace Aero.Application.Interface
{
    public interface IAccessAreaService
    {
        Task<ResponseDto<IEnumerable<AccessAreaDto>>> GetAsync();
        Task<ResponseDto<AccessAreaDto>> GetByComponentAsync(short component);
        Task<ResponseDto<IEnumerable<AccessAreaDto>>> GetByLocationIdAsync(short location);
        Task<ResponseDto<Pagination<AccessAreaDto>>> GetPaginationAsync(PaginationParamsWithFilter param,short location);
        Task<ResponseDto<AccessAreaDto>> CreateAsync(AccessAreaDto dto);
        Task<ResponseDto<AccessAreaDto>> DeleteAsync(short component);
        Task<ResponseDto<AccessAreaDto>> UpdateAsync(AccessAreaDto dto);
        Task<ResponseDto<IEnumerable<ModeDto>>> GetCommandAsync();
        Task<ResponseDto<IEnumerable<ModeDto>>> GetAccessControlOptionAsync();
        Task<ResponseDto<IEnumerable<ModeDto>>> GetOccupancyControlOptionAsync();
        Task<ResponseDto<IEnumerable<ModeDto>>> GetAreaFlagOptionAsync();
        Task<ResponseDto<IEnumerable<ModeDto>>> GetMultiOccupancyOptionAsync();
    }
}
