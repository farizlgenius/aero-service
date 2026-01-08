using HIDAeroService.Data;
using HIDAeroService.DTO;
using HIDAeroService.DTO.Feature;
using HIDAeroService.Helpers;
using HIDAeroService.Mapper;
using Microsoft.AspNetCore.Http.Headers;
using Microsoft.EntityFrameworkCore;

namespace HIDAeroService.Service.Impl
{
    public sealed class FeatureService(AppDbContext context) : IFeatureService
    {
        public async Task<ResponseDto<IEnumerable<FeatureDto>>> GetFeatureByRoleAsync(short RoleId)
        {
            var dtos = await context.feature_role
                .AsNoTracking()
                .Include(f => f.feature)
                .Where(f => f.role_id == RoleId)
                .Select(x => MapperHelper.FeatureToDto(x.feature,x.is_allow,x.is_create,x.is_modify,x.is_delete,x.is_action))
                .ToArrayAsync();

            return ResponseHelper.SuccessBuilder<IEnumerable<FeatureDto>>(dtos);
        }

        public async Task<ResponseDto<IEnumerable<FeatureDto>>> GetFeatureListAsync()
        {
            var dtos = await context.feature
                .AsNoTracking()
                .Select(x => new FeatureDto
                {
                    Name = x.name,
                    ComponentId = x.component_id,
                    IsAllow = false,
                    IsCreate =false,
                    IsModify = false,
                    IsDelete = false

                }).ToArrayAsync();
            return ResponseHelper.SuccessBuilder<IEnumerable<FeatureDto>>(dtos);
        }

        public async Task<ResponseDto<FeatureDto>> GetOneFeatureByRoleIdAsync(short RoleId, short FeatureId)
        {
            var dto = await context.feature_role
                .AsNoTracking()
                .Include(f => f.feature)
                .Where(f => f.role_id == RoleId && f.feature_id == FeatureId)
                .OrderBy(x => x.role_id)
                .Select(x => MapperHelper.FeatureToDto(x.feature,x.is_allow,x.is_create,x.is_modify,x.is_delete,x.is_action))
                .FirstOrDefaultAsync();

            return ResponseHelper.SuccessBuilder<FeatureDto>(dto);
        }
    }
}
