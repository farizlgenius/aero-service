using System;
using Aero.Application.DTOs;
using Aero.Domain.Entities;

namespace Aero.Application.Mapper;

public sealed class SettingMapper
{
      public static PasswordRule ToDomainPasswordRule(PasswordRuleDto dto)
      {
            return new PasswordRule
            {
                  Len = dto.Len,
                  IsLower = dto.IsLower,
                  IsUpper = dto.IsUpper,
                  IsDigit = dto.IsDigit,
                  IsSymbol = dto.IsSymbol,
                  Weaks = dto.Weaks
            };
      }

}
