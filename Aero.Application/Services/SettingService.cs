using Aero.Application.DTOs;
using Aero.Application.Helpers;
using Aero.Application.Interface;

namespace Aero.Application.Services
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
