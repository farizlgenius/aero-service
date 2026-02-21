using Aero.Application.DTOs;
using Aero.Domain.Entities;
using System;

namespace Aero.Application.Interface;

public interface ISettingRepository
{
      Task<int> UpdatePasswordRuleAsync(PasswordRule data);
    Task<PasswordRuleDto> GetPasswordRuleAsync();
    Task<bool> IsAnyPasswordRule();
}
