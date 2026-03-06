
using Aero.Application.DTOs;
using Aero.Domain.Entities;

namespace Aero.Application.Interface
{
    public interface ILocationService
    {
        Task<ResponseDto<IEnumerable<LocationDto>>> GetAsync();
        Task<ResponseDto<Pagination<LocationDto>>> GetPaginationAsync(PaginationParamsWithFilter param,int location);
        Task<ResponseDto<LocationDto>> GetByComponentIdAsync(int id);
        Task<ResponseDto<LocationDto>> CreateAsync(LocationDto dto);
        Task<ResponseDto<LocationDto>> DeleteByIdAsync(int id);
        Task<ResponseDto<IEnumerable<LocationDto>>> DeleteRangeAsync(List<int> ids);
        Task<ResponseDto<LocationDto>> UpdateAsync(LocationDto dto);
        Task<ResponseDto<IEnumerable<LocationDto>>> GetRangeLocationById(LocationRangeDto locationIds);
    }
}
