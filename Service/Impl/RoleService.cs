using HIDAeroService.Data;
using HIDAeroService.DTO;
using HIDAeroService.DTO.Role;
using HIDAeroService.Entity;
using HIDAeroService.Helpers;
using HIDAeroService.Mapper;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace HIDAeroService.Service.Impl
{
    public sealed class RoleService(AppDbContext context,IHelperService<Role> helperService) : IRoleService
    {
        public async Task<ResponseDto<bool>> CreateAsync(RoleDto dto)
        {
            if (await context.Roles.AsNoTracking().AnyAsync(r => r.Name == dto.Name)) return ResponseHelper.Duplicate<bool>();
            var ComponentId = await helperService.GetLowestUnassignedNumberNoLimitAsync<Role>(context);

            var en = MapperHelper.DtoToRole(dto,ComponentId,DateTime.Now);

            await context.Roles.AddAsync(en);
            await context.SaveChangesAsync();
            return ResponseHelper.SuccessBuilder<bool>(true);
        }

        public async Task<ResponseDto<bool>> DeleteByComponentIdAsync(short ComponentId)
        {
            var en = await context.Roles
                .Include(x => x.FeatureRoles)
                .Where(r => r.ComponentId == ComponentId)
                .OrderBy(x => x.ComponentId)
                .FirstOrDefaultAsync();

            if (en is null) return ResponseHelper.NotFoundBuilder<bool>();

            if (await context.Roles.AnyAsync(x => x.ComponentId == ComponentId && x.Operators.Any())) return ResponseHelper.FoundReferenceBuilder<bool>(["Found operator related"]);

            context.FeatureRoles.RemoveRange(en.FeatureRoles);

            context.Roles.Remove(en);
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
                await context.Roles
                .AsNoTracking()
                .Include(f => f.FeatureRoles)
                .ThenInclude(fr => fr.Feature)
                .ThenInclude(s => s.SubFeatures)
                .Select(x => MapperHelper.RoleToDto(x))
                .ToArrayAsync()
                );
        }

        public async Task<ResponseDto<RoleDto>> GetByComponentIdAsync(short ComponentId)
        {
            var dto = await context.Roles
                .AsNoTracking()
                .Include(f => f.FeatureRoles)
                .ThenInclude(fr => fr.Feature)
                .Select(x => MapperHelper.RoleToDto(x))
                .OrderBy(x => x.ComponentId)
                .FirstOrDefaultAsync();

            return ResponseHelper.SuccessBuilder(dto);
                
        }

        public async Task<ResponseDto<RoleDto>> UpdateAsync(RoleDto dto)
        {
            var en = await context.Roles
                .Include(f => f.FeatureRoles)
                .Where(r => r.ComponentId == dto.ComponentId)
                .OrderBy(r => r.ComponentId)
                .FirstOrDefaultAsync();

            if(en is null) return ResponseHelper.NotFoundBuilder<RoleDto>();

            // Delete Old Feature Role
            context.FeatureRoles.RemoveRange(en.FeatureRoles);

            MapperHelper.UpdateRole(en, dto);

            context.Roles.Update(en);
            await context.SaveChangesAsync();

            return ResponseHelper.SuccessBuilder(dto);
        }
    }
}
