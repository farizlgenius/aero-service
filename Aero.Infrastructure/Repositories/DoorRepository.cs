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

      public Task<int> DeleteByComponentIdAsync(short component)
      {
            throw new NotImplementedException();
      }

      public Task<int> UpdateAsync(Door newData)
      {
            throw new NotImplementedException();
      }
}
