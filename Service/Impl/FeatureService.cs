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
            var dtos = await context.FeatureRoles
                .AsNoTracking()
                .Include(f => f.Feature)
                .Where(f => f.RoleId == RoleId)
                .Select(x => MapperHelper.FeatureToDto(x.Feature,x.IsAllow,x.IsCreate,x.IsModify,x.IsDelete,x.IsAction))
                .ToArrayAsync();

            return ResponseHelper.SuccessBuilder<IEnumerable<FeatureDto>>(dtos);
        }

        public async Task<ResponseDto<IEnumerable<FeatureDto>>> GetFeatureListAsync()
        {
            var dtos = await context.Features
                .AsNoTracking()
                .Select(x => new FeatureDto
                {
                    Name = x.Name,
                    ComponentId = x.ComponentId,
                    IsAllow = false,
                    IsCreate =false,
                    IsModify = false,
                    IsDelete = false

                }).ToArrayAsync();
            return ResponseHelper.SuccessBuilder<IEnumerable<FeatureDto>>(dtos);
        }

        public async Task<ResponseDto<FeatureDto>> GetOneFeatureByRoleIdAsync(short RoleId, short FeatureId)
        {
            var dto = await context.FeatureRoles
                .AsNoTracking()
                .Include(f => f.Feature)
                .Where(f => f.RoleId == RoleId && f.FeatureId == FeatureId)
                .OrderBy(x => x.RoleId)
                .Select(x => MapperHelper.FeatureToDto(x.Feature,x.IsAllow,x.IsCreate,x.IsModify,x.IsDelete,x.IsAction))
                .FirstOrDefaultAsync();

            return ResponseHelper.SuccessBuilder<FeatureDto>(dto);
        }
    }
}
