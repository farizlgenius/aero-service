using System;
using Aero.Domain.Entities;

namespace Aero.Domain.Interface;

public interface ISettingRepository
{
      Task<int> UpdatePasswordRuleAsync(PasswordRule data);
}
