using HIDAeroService.Data;
using HIDAeroService.DTO;
using HIDAeroService.DTO.Location;

namespace HIDAeroService.Service.Impl
{
    public sealed class LocationService(AppDbContext context) : ILocation
    {
        public async Task<ResponseDto<bool>> Create()
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseDto<bool>> DeleteById(int Id)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseDto<IEnumerable<LocationDto>>> Get()
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseDto<LocationDto>> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseDto<LocationDto>> Update(LocationDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
