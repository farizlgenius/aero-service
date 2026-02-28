

using Aero.Application.DTOs;
using Aero.Domain.Entities;

namespace Aero.Application.Interface
{
    public interface IHolidayService
    {
        Task<ResponseDto<IEnumerable<HolidayDto>>> GetAsync();
        Task<ResponseDto<IEnumerable<HolidayDto>>> GetByLocationAsync(int location);
        Task<ResponseDto<Pagination<HolidayDto>>> GetPaginationAsync(PaginationParamsWithFilter param, int location);
        
        Task<ResponseDto<HolidayDto>> GetByIdAsync(int id);
        Task<ResponseDto<HolidayDto>> CreateAsync(HolidayDto dto);
        Task<ResponseDto<HolidayDto>> DeleteAsync(int id);
        Task<ResponseDto<HolidayDto>> UpdateAsync(HolidayDto dto);
        Task<ResponseDto<bool>> ClearAsync();
        Task<ResponseDto<IEnumerable<HolidayDto>>> DeleteRangeAsync( List<int> ids);
    }
}
