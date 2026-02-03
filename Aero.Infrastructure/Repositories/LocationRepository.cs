using System;
using Aero.Domain.Entities;
using Aero.Domain.Interface;
using Aero.Infrastructure.Data;
using Aero.Infrastructure.Mapper;
using Microsoft.EntityFrameworkCore;

namespace Aero.Infrastructure.Repositories;

public class LocationRepository(AppDbContext context) : ILocationRepository
{
      public async Task<int> AddAsync(Location data)
      {
            var en = LocationMapper.ToEf(data);
            await context.location.AddAsync(en);
            return await context.SaveChangesAsync();
      }

      public async Task<int> DeleteByComponentIdAsync(short component)
      {
            var en = await context.location
            .OrderBy(x => x.component_id)
            .Where(x => x.component_id == component)
            .FirstOrDefaultAsync();

            if(en is null) return 0;

            context.location.Remove(en);
            return await context.SaveChangesAsync();
      }

      public async Task<int> UpdateAsync(Location newData)
      {
            var en = await context.location
            .OrderBy(x => x.component_id)
            .Where(x => x.component_id == newData.ComponentId)
            .FirstOrDefaultAsync();

            if(en is null) return 0;

            LocationMapper.Update(newData,en);

            context.location.Update(en);
            return await context.SaveChangesAsync();
      }
}
