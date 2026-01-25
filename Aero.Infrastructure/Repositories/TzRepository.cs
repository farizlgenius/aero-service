using System;
using Aero.Domain.Entities;
using Aero.Domain.Interface;
using Aero.Infrastructure.Data;
using Aero.Infrastructure.Mapper;
using Microsoft.EntityFrameworkCore;

namespace Aero.Infrastructure.Repositories;

public class TzRepository(AppDbContext context) : ITzRepository
{
      public async Task<int> AddAsync(Timezone data,short ComponentId)
      {     
            data.ComponentId = ComponentId;
            await context.timezone.AddAsync(TimezoneMapper.ToEf(data));
            return await context.SaveChangesAsync();
      }

      public async Task<int> DeleteByComponentIdAsync(short component)
      {
            var en = await context.timezone
            .Where(x => x.component_id == component)
            .OrderBy(x => x.component_id)
            .FirstOrDefaultAsync();

            if(en is null) return 0;

            context.timezone.Remove(en);
            return await context.SaveChangesAsync();
      }

      public async Task<int> UpdateAsync(Timezone d)
      {
            var en = await context.timezone
            .Where(x => x.component_id == d.ComponentId)
            .OrderBy(x => x.component_id)
            .FirstOrDefaultAsync();

            if(en is null) return 0;

            // Update Process
            en.name = d.Name;
            en.mode = d.Mode;
            en.active_time = d.ActiveTime;
            en.deactive_time = d.DeactiveTime;

            context.timezone.Update(en);

            return await context.SaveChangesAsync();
      }
}
