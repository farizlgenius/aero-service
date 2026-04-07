

using Aero.Application.DTOs;

namespace Aero.Application.Interface
{
    public interface ISettingService
    {
        Task<ResponseDto<PasswordRuleDto>> GetPasswordRuleAsync();
        Task<ResponseDto<PasswordRuleDto>> UpdatePasswordRuleAsync(PasswordRuleDto dto);
        Task<ResponseDto<IEnumerable<LedDto>>> GetLedSettingAsync();
        Task<ResponseDto<LedDto>> GetLedSettingByIdAsync(int Id);
        Task<ResponseDto<LedDto>> UpdateLedSettingAsync(LedDto dto);
    }
}
