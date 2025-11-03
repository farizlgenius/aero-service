using HIDAeroService.DTO;
using HIDAeroService.DTO.Holiday;

namespace HIDAeroService.Service
{
    public interface IHolidayService
    {
        Task<ResponseDto<IEnumerable<HolidayDto>>> GetAsync();
        Task<ResponseDto<HolidayDto>> GetByComponentIdAsync(short component);
        Task<ResponseDto<bool>> CreateAsync(CreateHolidayDto dto);
        Task<ResponseDto<bool>> DeleteAsync(short component);
        Task<ResponseDto<HolidayDto>> UpdateAsync(HolidayDto dto);
        Task<ResponseDto<bool>> ClearAsync();
    }
}
