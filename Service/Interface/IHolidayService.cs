using HIDAeroService.Dto;
using HIDAeroService.Dto.Holiday;
using Microsoft.AspNetCore.Mvc;

namespace HIDAeroService.Service.Interface
{
    public interface IHolidayService
    {
        Task<Response<IEnumerable<HolidayDto>>> GetAsync();
        Task<Response<HolidayDto>> CreateAsync(HolidayDto dto);
        Task<Response<HolidayDto>> UpdateAsync(HolidayDto dto);
        Task<Response<HolidayDto>> DeleteAsync(short id);
        Task<Response<HolidayDto>> GetByIdAsync(short id);
    }
}
