using System;
using Aero.Application.DTOs;
using Aero.Application.Interfaces;
using Aero.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Aero.Infrastructure.Repositories;

public class QSettingRepository(AppDbContext context) : IQSettingRepository
{
      public async Task<PasswordRuleDto> GetPasswordRuleAsync()
      {
            var dto = await context.password_rule
                .AsNoTracking()
                .OrderBy(x => x.id)
                .Select(x => new PasswordRuleDto
                {
                  Len = x.len,
                  IsLower = x.is_lower,
                  IsUpper = x.is_upper,
                  IsDigit = x.is_digit,
                  IsSymbol = x.is_symbol,
                  Weaks = x.weaks.Select(x => x.pattern).ToList()
                })
                .FirstOrDefaultAsync();

            return dto;

      }

      public async Task<bool> IsAnyPasswordRule()
      {
            return await context.password_rule.AnyAsync();
      }
}
