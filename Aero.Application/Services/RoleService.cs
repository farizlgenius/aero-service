using System.Net;
using Aero.Application.DTOs;
using Aero.Application.Helpers;

namespace Aero.Application.Services
{
    public sealed class RoleService(AppDbContext context,IHelperService<Role> helperService) : IRoleService
    {
        public async Task<ResponseDto<bool>> CreateAsync(RoleDto dto)
        {
            if (await context.role.AsNoTracking().AnyAsync(r => r.name == dto.Name)) return ResponseHelper.Duplicate<bool>();
            var ComponentId = await helperService.GetLowestUnassignedNumberNoLimitAsync<Role>(context);

            var en = MapperHelper.DtoToRole(dto,ComponentId,DateTime.UtcNow);

            await context.role.AddAsync(en);
            await context.SaveChangesAsync();
            return ResponseHelper.SuccessBuilder<bool>(true);
        }

        public async Task<ResponseDto<bool>> DeleteByComponentIdAsync(short ComponentId)
        {
            var en = await context.role
                .Include(x => x.feature_roles)
                .Where(r => r.component_id == ComponentId)
                .OrderBy(x => x.component_id)
                .FirstOrDefaultAsync();

            if (en is null) return ResponseHelper.NotFoundBuilder<bool>();

            if (await context.role.AnyAsync(x => x.component_id == ComponentId && x.operators.Any())) return ResponseHelper.FoundReferenceBuilder<bool>(["Found operator related"]);

            context.feature_role.RemoveRange(en.feature_roles);

            context.role.Remove(en);
            await context.SaveChangesAsync();

            return ResponseHelper.SuccessBuilder<bool>(true);
        }

        public async Task<ResponseDto<IEnumerable<ResponseDto<bool>>>> DeleteRangeAsync(List<short> dtos)
        {
            bool flag = true;
            List<ResponseDto<bool>> data = new List<ResponseDto<bool>>();
            foreach (var dto in dtos)
            {
                var re = await DeleteByComponentIdAsync(dto);
                if (re.code != HttpStatusCode.OK) flag = false;
                data.Add(re);
            }

            if (!flag) return ResponseHelper.UnsuccessBuilder<IEnumerable<ResponseDto<bool>>>(data);

            var res = ResponseHelper.SuccessBuilder<IEnumerable<ResponseDto<bool>>>(data);

            return res;
        }

        public async Task<ResponseDto<IEnumerable<RoleDto>>> GetAsync()
        {
            return ResponseHelper.SuccessBuilder<IEnumerable<RoleDto>>(
                await context.role
                .AsNoTracking()
                .Select(x => new RoleDto
                {
                    component_id = x.component_id,
                    Name = x.name,
                    Features = x.feature_roles.Count > 0 ? x.feature_roles.Select(a => MapperHelper.FeatureToDto(a.feature, a.is_allow, a.is_create, a.is_modify, a.is_delete, a.is_action)).ToList() : new List<FeatureDto>()
                })
                .ToArrayAsync()
                );
        }

        public async Task<ResponseDto<RoleDto>> GetByComponentIdAsync(short ComponentId)
        {
            var dto = await context.role
                .AsNoTracking()
                .Select(x => new RoleDto
                {
                    component_id = x.component_id,
                    Name = x.name,
                    Features = x.feature_roles.Count > 0 ? x.feature_roles.Select(a => MapperHelper.FeatureToDto(a.feature, a.is_allow, a.is_create, a.is_modify, a.is_delete, a.is_action)).ToList() : new List<FeatureDto>()
                })
                .OrderBy(x => x.component_id)
                .FirstOrDefaultAsync();

            if (dto is null) return ResponseHelper.NotFoundBuilder<RoleDto>();

            return ResponseHelper.SuccessBuilder(dto);
                
        }

        public async Task<ResponseDto<RoleDto>> UpdateAsync(RoleDto dto)
        {
            var en = await context.role
                .Include(f => f.feature_roles)
                .Where(r => r.component_id == dto.component_id)
                .OrderBy(r => r.component_id)
                .FirstOrDefaultAsync();

            if(en is null) return ResponseHelper.NotFoundBuilder<RoleDto>();

            // Delete Old feature role
            context.feature_role.RemoveRange(en.feature_roles);

            MapperHelper.UpdateRole(en, dto);

            context.role.Update(en);
            await context.SaveChangesAsync();

            return ResponseHelper.SuccessBuilder(dto);
        }
    }
}
