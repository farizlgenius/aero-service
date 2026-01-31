using System;
using Aero.Domain.Entities;
using Aero.Domain.Interface;
using Aero.Infrastructure.Data;
using Aero.Infrastructure.Mapper;
using Microsoft.EntityFrameworkCore;

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
}
