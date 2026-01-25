

using Aero.Application.DTOs;

namespace Aero.Application.Interface
{
    public interface IHolidayService
    {
        Task<ResponseDto<IEnumerable<HolidayDto>>> GetAsync();
        Task<ResponseDto<IEnumerable<HolidayDto>>> GetByLocationAsync(short location);
        
        Task<ResponseDto<HolidayDto>> GetByComponentIdAsync(short component);
        Task<ResponseDto<bool>> CreateAsync(HolidayDto dto);
        Task<ResponseDto<bool>> DeleteAsync(short component);
        Task<ResponseDto<HolidayDto>> UpdateAsync(HolidayDto dto);
        Task<ResponseDto<bool>> ClearAsync();
        Task<ResponseDto<IEnumerable<ResponseDto<bool>>>> DeleteRangeAsync( List<short> components);
    }
}
