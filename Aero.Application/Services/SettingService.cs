using System.Diagnostics;
using Aero.Application.DTOs;
using Aero.Application.Helpers;
using Aero.Application.Interface;
using Aero.Application.Interfaces;
using Aero.Domain.Interface;

namespace Aero.Application.Services
{
    public sealed class SettingService(ISettingRepository repo) : ISettingService
    {
        public async Task<ResponseDto<PasswordRuleDto>> GetPasswordRuleAsync()
        {
            var dto = await repo.GetPasswordRuleAsync();
            return ResponseHelper.SuccessBuilder<PasswordRuleDto>(dto);
        }

        public async Task<ResponseDto<PasswordRuleDto>> UpdatePasswordRuleAsync(PasswordRuleDto dto)
        {

            if (!await repo.IsAnyPasswordRule()) return ResponseHelper.NotFoundBuilder<PasswordRuleDto>();

            var domain = new Domain.Entities.PasswordRule(dto.Len,dto.IsLower,dto.IsUpper,dto.IsDigit,dto.IsSymbol,dto.Weaks);

            var status = await repo.UpdatePasswordRuleAsync(domain);

            return ResponseHelper.SuccessBuilder<PasswordRuleDto>(dto);
        }
    }
}
