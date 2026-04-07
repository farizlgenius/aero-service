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
            en.Update(data);
            context.password_rule.Update(en);
            return await context.SaveChangesAsync();
      }

    public async Task<PasswordRuleDto> GetPasswordRuleAsync()
    {
        var data = await context.password_rule
            .AsNoTracking()
            .OrderBy(x => x.id)
            .Select(x => new {x.len,x.is_lower,x.is_upper,x.is_digit,x.is_symbol,weaks = x.weaks.Select(x => x.pattern)})
            .FirstOrDefaultAsync();

            var res = new PasswordRuleDto(
                data.len,
                data.is_lower,
                data.is_upper,
                data.is_digit,
                data.is_symbol,data.weaks.ToList());

        return res;

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

      public async Task<IEnumerable<LedDto>> GetLedSettingAsync()
      {
            var en = await context.led
            .AsNoTracking()
            .Select(x => new LedDto(
                x.id,
                x.led_mode,
                x.config.Select(c => new LedConfigDto(
                    c.r_led_id,
                    c.on_color,
                    c.off_color,
                    c.on_time,
                    c.off_time,
                    c.repeat_count,
                    c.beep_count
                )).ToList(),
                x.location_id,
                x.is_active
            ))
            .ToArrayAsync();

            return en;
      }

      public async Task<LedDto> GetLedByIdAsync(int id)
      {
            var en = await context.led
            .AsNoTracking()
            .Where(x => x.id == id)
                        .OrderBy(x => x.id)
            .Select(x => new LedDto(
                x.id,
                x.led_mode,
                x.config.Select(c => new LedConfigDto(
                    c.r_led_id,
                    c.on_color,
                    c.off_color,
                    c.on_time,
                    c.off_time,
                    c.repeat_count,
                    c.beep_count
                )).ToList(),
                x.location_id,
                x.is_active
            ))
            .FirstOrDefaultAsync();

            if(en is null) return null;

            return en;
      }

      public async Task<int> UpdateLedSettingAsync(Led data)
      {
            var en = await context.led
            .Where(x => x.id == data.Id)
            .OrderBy(x => x.id)
            .FirstOrDefaultAsync();

            if(en is null) return -1;

            // Remove old
            var configs = await context.led_config
            .Where(x => x.led_id == en.id)
            .ToArrayAsync();

            context.led_config.RemoveRange(configs);

            en.Update(data);

            return await context.SaveChangesAsync();
      }
}
