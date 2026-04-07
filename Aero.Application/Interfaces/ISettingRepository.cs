using Aero.Application.DTOs;
using Aero.Domain.Entities;
using System;

namespace Aero.Application.Interface;

public interface ISettingRepository
{
    Task<ScpSetting> GetScpSettingAsync();
      Task<int> UpdatePasswordRuleAsync(PasswordRule data);
      Task<int> UpdateLedSettingAsync(Led data);
    Task<PasswordRuleDto> GetPasswordRuleAsync();
    Task<bool> IsAnyPasswordRule();
    Task<IEnumerable<LedDto>> GetLedSettingAsync();
    Task<LedDto> GetLedByIdAsync(int id);

}
