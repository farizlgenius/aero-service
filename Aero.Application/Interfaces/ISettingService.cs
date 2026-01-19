

using Aero.Application.DTOs;

namespace Aero.Application.Interface
{
    public interface ISettingService
    {
        Task<ResponseDto<PasswordRuleDto>> GetPasswordRuleAsync();
        Task<ResponseDto<PasswordRuleDto>> UpdatePasswordRuleAsync(PasswordRuleDto dto);
    }
}
