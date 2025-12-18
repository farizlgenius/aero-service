using HIDAeroService.DTO;
using HIDAeroService.DTO.Location;

namespace HIDAeroService.Service
{
    public interface ILocationService
    {
        Task<ResponseDto<IEnumerable<LocationDto>>> GetAsync();
        Task<ResponseDto<LocationDto>> GetByIdAsync(short component);
        Task<ResponseDto<bool>> CreateAsync(LocationDto dto);
        Task<ResponseDto<bool>> DeleteByIdAsync(short component);
        Task<ResponseDto<IEnumerable<ResponseDto<bool>>>> DeleteRangeAsync(List<short> components);
        Task<ResponseDto<LocationDto>> UpdateAsync(LocationDto dto);
        Task<ResponseDto<IEnumerable<LocationDto>>> GetRangeLocationById(LocationRangeDto locationIds);
    }
}
