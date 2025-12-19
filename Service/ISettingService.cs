using HIDAeroService.DTO.Operator;
using HIDAeroService.DTO;

namespace HIDAeroService.Service
{
    public interface ISettingService
    {
        Task<ResponseDto<PasswordRuleDto>> GetPasswordRuleAsync();
        Task<ResponseDto<PasswordRuleDto>> UpdatePasswordRuleAsync(PasswordRuleDto dto);
    }
}
