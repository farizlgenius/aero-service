using AeroService.DTO.Operator;
using AeroService.DTO;

namespace AeroService.Service
{
    public interface ISettingService
    {
        Task<ResponseDto<PasswordRuleDto>> GetPasswordRuleAsync();
        Task<ResponseDto<PasswordRuleDto>> UpdatePasswordRuleAsync(PasswordRuleDto dto);
    }
}
