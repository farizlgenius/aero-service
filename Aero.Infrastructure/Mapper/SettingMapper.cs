using System;
using Aero.Infrastructure.Data.Entities;

namespace Aero.Infrastructure.Mapper;

public sealed  class SettingMapper
{
      public static void PasswordRuleUpdate(Aero.Domain.Entities.PasswordRule from,Aero.Infrastructure.Data.Entities.PasswordRule to)
      {
            to.len = from.Len;
            to.is_lower = from.IsLower;
            to.is_upper = from.IsUpper;
            to.is_digit = from.IsDigit;
            to.is_symbol = from.IsSymbol;
            to.weaks = from.Weaks.Select(x => new WeakPassword
            {
                  pattern = x,
            }).ToList();
      }

}
