using System;
using Aero.Application.DTOs;

namespace Aero.Application.Interfaces;

public interface IQSettingRepository
{
      Task<PasswordRuleDto> GetPasswordRuleAsync();
      Task<bool> IsAnyPasswordRule();
}
