using Aero.Application.DTOs;
using Aero.Domain.Entities;
using Aero.Domain.Interface;
using Aero.Infrastructure.Data;
using Aero.Infrastructure.Mapper;
using Microsoft.EntityFrameworkCore;
using System;

namespace Aero.Infrastructure.Repositories;

public class SettingRepository(AppDbContext context) : ISettingRepository
{
      public async Task<int> UpdatePasswordRuleAsync(PasswordRule data)
      {
            var en = await context.password_rule.OrderBy(x => x.id).FirstOrDefaultAsync();
            if(en is null) return 0;
            SettingMapper.PasswordRuleUpdate(data,en);
            context.password_rule.Update(en);
            return await context.SaveChangesAsync();
      }

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
