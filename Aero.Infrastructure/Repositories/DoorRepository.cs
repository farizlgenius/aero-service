using System;
using Aero.Domain.Entities;
using Aero.Domain.Interface;
using Aero.Infrastructure.Data;
using Aero.Infrastructure.Mapper;
using Microsoft.EntityFrameworkCore;

namespace Aero.Infrastructure.Repositories;

public class DoorRepository(AppDbContext context) : IDoorRepository
{
      public async Task<int> AddAsync(Door data)
      {
            await context.access_level_door_component.AddAsync(new Data.Entities.AccessLevelDoorComponent
            {
                door_id = data.ComponentId,
                acr_id = data.AcrId,
                timezone_id = 1,
                access_level_component = new Data.Entities.AccessLevelComponent 
                {
                    mac = data.Mac,
                    access_level_id = 2
                }
            });
            var en = DoorMapper.ToEf(data);
            await context.door.AddAsync(en);
            return await context.SaveChangesAsync();
      }

      public async Task<int> ChangeDoorModeAsync(string mac,short component,short acr,short mode)
      {
            var en = await context.door
            .Where(x => x.acr_id == acr && x.mac == mac && x.component_id == component)
            .OrderBy(x => x.component_id)
            .FirstOrDefaultAsync();

            if(en is null) return 0;

            en.mode = mode;
            en.mode_desc = await context.door_mode
                .AsNoTracking()
                .Where(x => x.value == en.mode)
                .Select(x => x.name)
                .FirstOrDefaultAsync() ?? "";
            context.Update(en);
            return await context.SaveChangesAsync();
      }

      public async Task<int> DeleteByComponentIdAsync(short component)
      {
        var en = await context.door
        .Where(x => x.component_id == component)
        .OrderBy(x => x.component_id)
        .FirstOrDefaultAsync();

        if (en is null) return 0;

        context.door.Remove(en);
        return await context.SaveChangesAsync();
      }

      public async Task<int> UpdateAsync(Door newData)
      {
            var en = await context.door
            .Include(x => x.readers)
            .Include(x => x.sensor)
            .Include(x => x.request_exits)
            .Include(x => x.strike)
            .Where(x => x.component_id == newData.ComponentId)
            .OrderBy(x => x.component_id)
            .FirstOrDefaultAsync();

            if(en is null) return 0;

            // Delete old component
            context.reader.RemoveRange(en.readers);
            context.sensor.Remove(en.sensor);
            context.strike.Remove(en.strike);
            if(en.request_exits != null) context.request_exit.RemoveRange(en.request_exits);

            DoorMapper.Update(newData,en);

            return await context.SaveChangesAsync();

      }
}
