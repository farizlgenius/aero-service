using HIDAeroService.Data;
using HIDAeroService.DTO;
using HIDAeroService.DTO.Role;
using HIDAeroService.Helpers;
using HIDAeroService.Mapper;
using Microsoft.EntityFrameworkCore;

namespace HIDAeroService.Service.Impl
{
    public sealed class RoleService(AppDbContext context) : IRoleService
    {
        public async Task<ResponseDto<bool>> CreateAsync(RoleDto dto)
        {
            if (await context.Roles.AsNoTracking().AnyAsync(r => r.Name == dto.Name)) return ResponseHelper.Duplicate<bool>();

            var en = MapperHelper.DtoToRole(dto);

            await context.Roles.AddAsync(en);
            await context.SaveChangesAsync();
            return ResponseHelper.SuccessBuilder<bool>(true);
        }

        public async Task<ResponseDto<bool>> DeleteByComponentIdAsync(short ComponentId)
        {
            var en = await context.Roles
                .Where(r => r.ComponentId == ComponentId)
                .OrderBy(x => x.ComponentId)
                .FirstOrDefaultAsync();

            if (en is null) return ResponseHelper.NotFoundBuilder<bool>();

            context.Roles.Remove(en);
            await context.SaveChangesAsync();

            return ResponseHelper.SuccessBuilder<bool>(true);
        }

        public async Task<ResponseDto<IEnumerable<RoleDto>>> GetAsync()
        {
            return ResponseHelper.SuccessBuilder<IEnumerable<RoleDto>>(
                await context.Roles
                .AsNoTracking()
                .Include(f => f.FeatureRoles)
                .Select(x => MapperHelper.RoleToDto(x))
                .ToArrayAsync()
                );
        }

        public async Task<ResponseDto<RoleDto>> GetByComponentIdAsync(short ComponentId)
        {
            var dto = await context.Roles
                .AsNoTracking()
                .Include(f => f.FeatureRoles)
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

            MapperHelper.UpdateRole(en, dto);

            context.Roles.Update(en);
            await context.SaveChangesAsync();

            return ResponseHelper.SuccessBuilder(dto);
        }
    }
}
