using System;
using Aero.Application.Mapper;
using Aero.Domain.Entities;
using Aero.Domain.Interface;
using Aero.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Aero.Infrastructure.Repositories;

public class AreaRepository(AppDbContext context) : IAreaRepository
{
      public async Task<int> AddAsync(AccessArea data)
      {
            var en = Aero.Infrastructure.Mapper.AreaMapper.ToEf(data);
            await context.area.AddAsync(en);
            return await context.SaveChangesAsync();
      }

      public async Task<int> DeleteByComponentIdAsync(short component)
      {
             var en = await context.area
            .Where(x => x.component_id == component)
            .OrderBy(x => x.component_id)
            .FirstOrDefaultAsync();

             if(en is null) return 0;

            context.area.Remove(en);
            return await context.SaveChangesAsync();

      }

      public async Task<int> UpdateAsync(AccessArea newData)
      {
            var en = await context.area
            .Where(x => x.component_id == newData.ComponentId)
            .OrderBy(x => x.component_id)
            .FirstOrDefaultAsync();

            if(en is null) return 0;

            Aero.Infrastructure.Mapper.AreaMapper.Update(newData,en);

            context.area.Update(en);
            return await context.SaveChangesAsync();
      }
}
