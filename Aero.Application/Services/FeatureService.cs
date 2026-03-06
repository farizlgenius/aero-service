using Aero.Application.DTOs;
using Aero.Application.Helpers;
using Aero.Application.Interface;
using Aero.Application.Interfaces;

namespace Aero.Application.Services
{
    public sealed class FeatureService(IFeatureRepository repo) : IFeatureService
    {
        public async Task<ResponseDto<IEnumerable<FeatureDto>>> GetFeatureByRoleAsync(short RoleId)
        {
            var dtos = await repo.GetFeatureByRoleAsync(RoleId);

            return ResponseHelper.SuccessBuilder<IEnumerable<FeatureDto>>(dtos);
        }

        public async Task<ResponseDto<IEnumerable<FeatureDto>>> GetFeatureListAsync()
        {
            var dtos = await repo.GetAsync();
            return ResponseHelper.SuccessBuilder<IEnumerable<FeatureDto>>(dtos);
        }

        public async Task<ResponseDto<FeatureDto>> GetFeatureByRoleIdAndFeatureIdAsync(short RoleId, short FeatureId)
        {
            var dto = await repo.GetFeatureByRoleIdAndFeatureIdAsync(RoleId,FeatureId);
            return ResponseHelper.SuccessBuilder<FeatureDto>(dto);
        }
    }
}
