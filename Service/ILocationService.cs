using HIDAeroService.DTO;
using HIDAeroService.DTO.Location;

namespace HIDAeroService.Service
{
    public interface ILocationService
    {
        Task<ResponseDto<IEnumerable<LocationDto>>> GetAsync();
        Task<ResponseDto<LocationDto>> GetByIdAsync(short Id);
        Task<ResponseDto<bool>> CreateAsync(LocationDto dto);
        Task<ResponseDto<bool>> DeleteByIdAsync(short Id);
        Task<ResponseDto<LocationDto>> UpdateAsync(LocationDto dto);
        Task<ResponseDto<IEnumerable<LocationDto>>> GetRangeLocationById(LocationRangeDto locationIds);
    }
}
