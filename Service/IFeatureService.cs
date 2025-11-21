using HIDAeroService.DTO;
using HIDAeroService.DTO.Feature;

namespace HIDAeroService.Service
{
    public interface IFeatureService
    {
        Task<ResponseDto<IEnumerable<FeatureDto>>> GetFeatureListAsync();
    }
}
