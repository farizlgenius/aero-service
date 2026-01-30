using System;
using Aero.Domain.Entities;
using Aero.Domain.Interface;
using Aero.Infrastructure.Data;
using Aero.Infrastructure.Mapper;
using Microsoft.EntityFrameworkCore;

namespace Aero.Infrastructure.Repositories;

public sealed class MpRepository(AppDbContext context) : IMpRepository
{
      public async Task<int> AddAsync(MonitorPoint data)
      {
            var en = MonitorPointMapper.ToEf(data);

            await context.monitor_point.AddAsync(en);
            return await context.SaveChangesAsync();
      }

      public async Task<int> DeleteByComponentIdAsync(short component)
      {
            var en = await context.monitor_point
            .Where(x => x.component_id == component)
            .OrderBy(x => x.component_id)
            .FirstOrDefaultAsync();

            if(en is null) return 0;

            context.monitor_point.Remove(en);
            return await context.SaveChangesAsync();
      }

      public async Task<int> SetMaskAsync(string mac,short mpid,bool mask)
      {
            var en = await context.monitor_point
            .Where(x => x.mp_id == mpid && x.mac.Equals(mac))
            .OrderBy(x => x.component_id)
            .FirstOrDefaultAsync();

            if(en is null) return 0;

            en.is_mask = mask;
            context.monitor_point.Update(en);
            return await context.SaveChangesAsync();
      }

      public async Task<int> UpdateAsync(MonitorPoint newData)
      {
            var en = await context.monitor_point
            .Where(x => x.component_id == newData.ComponentId && x.mp_id == newData.MpId && x.mac.Equals(newData.Mac))
            .OrderBy(x => x.component_id)
            .FirstOrDefaultAsync();

            if(en is null) return 0;

            MonitorPointMapper.Update(newData,en);

            context.monitor_point.Update(en);
            return await context.SaveChangesAsync();
      }
}
