using AeroService.DTO.Operator;
using AeroService.DTO;
using AeroService.Helpers;
using AeroService.Mapper;
using AeroService.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace AeroService.Service.Impl
{
    public sealed class SettingService(AppDbContext context) : ISettingService
    {
        public async Task<ResponseDto<PasswordRuleDto>> GetPasswordRuleAsync()
        {
            var dto = await context.password_rule
                .AsNoTracking()
                .Include(x => x.weaks)
                .OrderBy(x => x.id)
                .Select(x => MapperHelper.PasswordRuleToDto(x))
                .FirstOrDefaultAsync();

            return ResponseHelper.SuccessBuilder<PasswordRuleDto>(dto);
        }

        public async Task<ResponseDto<PasswordRuleDto>> UpdatePasswordRuleAsync(PasswordRuleDto dto)
        {
            var en = await context.password_rule
                .OrderBy(x => x.id)
                .Include(x => x.weaks)
                .FirstOrDefaultAsync();

            if (en is null) return ResponseHelper.NotFoundBuilder<PasswordRuleDto>();

            MapperHelper.UpdatePasswordRule(en, dto);

            context.password_rule.Update(en);

            await context.SaveChangesAsync();

            return ResponseHelper.SuccessBuilder<PasswordRuleDto>(dto);
        }
    }
}
