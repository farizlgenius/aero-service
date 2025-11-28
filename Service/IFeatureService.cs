using HIDAeroService.DTO;
using HIDAeroService.DTO.Feature;

namespace HIDAeroService.Service
{
    public interface IFeatureService
    {
        Task<ResponseDto<IEnumerable<FeatureDto>>> GetFeatureListAsync();
        Task<ResponseDto<IEnumerable<FeatureDto>>> GetFeatureByRoleAsync(short RoleId);
        Task<ResponseDto<FeatureDto>> GetOneFeatureByRoleIdAsync(short RoleId,short FeatureId);
    }
}
