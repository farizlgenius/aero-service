using System;
using Aero.Domain.Entities;
using Aero.Domain.Interface;
using Aero.Infrastructure.Data;
using Aero.Infrastructure.Mapper;
using Microsoft.EntityFrameworkCore;

namespace Aero.Infrastructure.Services;

public class MpgRepository(AppDbContext context) : IMpgRepository
{
      public async Task<int> AddAsync(MonitorGroup data)
      {
            var en = MonitorGroupMapper.ToEf(data);
            await context.monitor_group.AddAsync(en);
            return await context.SaveChangesAsync();
      }


      public Task<int> DeleteByComponentIdAsync(short component)
      {
            throw new NotImplementedException();
      }

      public async Task<int> DeleteByMacAndComponentIdAsync(string mac, short component)
      {
            var en = await context.monitor_group
            .Where(x => x.mac.Equals(mac) && x.component_id == component)
            .FirstOrDefaultAsync();

            if(en is null) return 0;

            context.monitor_group.Remove(en);
            return await context.SaveChangesAsync();
      }

      public async Task<int> DeleteReferenceByMacAnsComponentIdAsync(string mac, short component)
      {
            var en = await context.monitor_group_list.Where(x => x.monitor_group_id == component).ToArrayAsync();

            context.monitor_group_list.RemoveRange(en);
            return await context.SaveChangesAsync();
      }

      public async Task<int> UpdateAsync(MonitorGroup newData)
      {
            var en = await context.monitor_group
            .Where(x => x.component_id == newData.ComponentId && x.mac.Equals(newData.Mac))
            .FirstOrDefaultAsync();

            if(en is null) return 0;

            MonitorGroupMapper.Update(newData,en);

            context.monitor_group.Update(en);
            return await context.SaveChangesAsync();
      }
}
