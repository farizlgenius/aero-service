using HIDAeroService.DTO.Operator;
using HIDAeroService.DTO;
using HIDAeroService.Helpers;
using HIDAeroService.Mapper;
using HIDAeroService.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace HIDAeroService.Service.Impl
{
    public sealed class SettingService(AppDbContext context) : ISettingService
    {
        public async Task<ResponseDto<PasswordRuleDto>> GetPasswordRuleAsync()
        {
            var dto = await context.PasswordRules
                .AsNoTracking()
                .Include(x => x.Weaks)
                .OrderBy(x => x.Id)
                .Select(x => MapperHelper.PasswordRuleToDto(x))
                .FirstOrDefaultAsync();

            return ResponseHelper.SuccessBuilder<PasswordRuleDto>(dto);
        }

        public async Task<ResponseDto<PasswordRuleDto>> UpdatePasswordRuleAsync(PasswordRuleDto dto)
        {
            var en = await context.PasswordRules
                .OrderBy(x => x.Id)
                .Include(x => x.Weaks)
                .FirstOrDefaultAsync();

            if (en is null) return ResponseHelper.NotFoundBuilder<PasswordRuleDto>();

            MapperHelper.UpdatePasswordRule(en, dto);

            context.PasswordRules.Update(en);

            await context.SaveChangesAsync();

            return ResponseHelper.SuccessBuilder<PasswordRuleDto>(dto);
        }
    }
}
