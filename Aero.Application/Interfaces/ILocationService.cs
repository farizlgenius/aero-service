
using Aero.Application.DTOs;
using Aero.Domain.Entities;

namespace Aero.Application.Interface
{
    public interface ILocationService
    {
        Task<ResponseDto<IEnumerable<LocationDto>>> GetAsync();
        Task<ResponseDto<Pagination<LocationDto>>> GetPaginationAsync(PaginationParamsWithFilter param,short location);
        Task<ResponseDto<LocationDto>> GetByComponentIdAsync(short component);
        Task<ResponseDto<bool>> CreateAsync(LocationDto dto);
        Task<ResponseDto<bool>> DeleteByComponentIdAsync(short component);
        Task<ResponseDto<IEnumerable<ResponseDto<bool>>>> DeleteRangeAsync(List<short> components);
        Task<ResponseDto<LocationDto>> UpdateAsync(LocationDto dto);
        Task<ResponseDto<IEnumerable<LocationDto>>> GetRangeLocationById(LocationRangeDto locationIds);
    }
}
