using HIDAeroService.DTO;
using HIDAeroService.DTO.Location;

namespace HIDAeroService.Service
{
    public interface ILocation
    {
        Task<ResponseDto<IEnumerable<LocationDto>>> Get();
        Task<ResponseDto<LocationDto>> GetById(int Id);
        Task<ResponseDto<bool>> Create();
        Task<ResponseDto<bool>> DeleteById(int Id);
        Task<ResponseDto<LocationDto>> Update(LocationDto dto);
    }
}
