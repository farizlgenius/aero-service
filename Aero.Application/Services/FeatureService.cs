using Aero.Application.DTOs;
using Aero.Application.Helpers;
using Aero.Application.Interface;
using Aero.Application.Interfaces;

namespace Aero.Application.Services
{
    public sealed class FeatureService(IQFeatureRepository qFeature) : IFeatureService
    {
        public async Task<ResponseDto<IEnumerable<FeatureDto>>> GetFeatureByRoleAsync(short RoleId)
        {
            var dtos = await qFeature.GetFeatureByRoleAsync(RoleId);

            return ResponseHelper.SuccessBuilder<IEnumerable<FeatureDto>>(dtos);
        }

        public async Task<ResponseDto<IEnumerable<FeatureDto>>> GetFeatureListAsync()
        {
            var dtos = await qFeature.GetAsync();
            return ResponseHelper.SuccessBuilder<IEnumerable<FeatureDto>>(dtos);
        }

        public async Task<ResponseDto<FeatureDto>> GetFeatureByRoleIdAndFeatureIdAsync(short RoleId, short FeatureId)
        {
            var dto = await qFeature.GetFeatureByRoleIdAndFeatureIdAsync(RoleId,FeatureId);
            return ResponseHelper.SuccessBuilder<FeatureDto>(dto);
        }
    }
}
