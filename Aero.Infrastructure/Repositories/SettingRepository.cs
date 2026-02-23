using Aero.Application.DTOs;
using Aero.Application.Interface;
using Aero.Domain.Entities;
using Aero.Infrastructure.Mapper;
using Aero.Infrastructure.Persistences;
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

    public async Task<ScpSetting> GetScpSettingAsync()
    {
        var en = await context.scp_setting.FirstOrDefaultAsync();
        return new ScpSetting(en.n_msp1_port,en.n_transaction,en.n_sio,en.n_mp,en.n_cp,en.n_acr,en.n_alvl,en.n_trgr,en.n_proc,en.gmt_offset,en.n_tz,en.n_hol,en.n_mpg,en.n_card,en.n_area,en.n_cfmt);
    }
}
