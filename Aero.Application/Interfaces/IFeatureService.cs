

using Aero.Application.DTOs;

namespace Aero.Application.Interface
{
    public interface IFeatureService
    {
        Task<ResponseDto<IEnumerable<FeatureDto>>> GetFeatureListAsync();
        Task<ResponseDto<IEnumerable<FeatureDto>>> GetFeatureByRoleAsync(short RoleId);
        Task<ResponseDto<FeatureDto>> GetOneFeatureByRoleIdAsync(short RoleId,short FeatureId);
    }
}
