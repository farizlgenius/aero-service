using System;
using Aero.Application.Mapper;
using Aero.Domain.Entities;
using Aero.Domain.Interface;
using Aero.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Aero.Infrastructure.Repositories;

public class AlvlRepository(AppDbContext context) : IAlvlRepository
{
      public async Task<int> AddAsync(AccessLevel data)
      {

            throw new NotImplementedException();
      }

      public async Task<int> AddCreateAsync(AccessLevel domain)
      {
            await context.access_level.AddAsync(Aero.Infrastructure.Mapper.AccessLevelMapper.ToEf(domain));
            return await context.SaveChangesAsync();
      }

      public async Task<int> DeleteByComponentIdAsync(short component)
      {
            var en = await context.access_level
            .Where(x => x.component_id == component)
            .OrderBy(x => x.component_id)
            .FirstOrDefaultAsync();

            if(en is null) return 0;

            context.access_level.Remove(en);
            return await context.SaveChangesAsync();
      }

      public async Task<int> UpdateAsync(AccessLevel newData)
      {
            throw new NotImplementedException();
      }

      public async Task<int> UpdateCreateAsync(AccessLevel domain)
      {
            var en = await context.access_level
            .Where(x => x.component_id == domain.ComponentId)
            .OrderBy(x => x.component_id)
            .FirstOrDefaultAsync();

            if(en is null) return 0;

            var e = await context.access_level_component
            .Where(x => x.access_level_id == domain.ComponentId)
            .ToArrayAsync();

            context.access_level_component.RemoveRange(e);

            Aero.Infrastructure.Mapper.AccessLevelMapper.Update(domain,en);

            return await context.SaveChangesAsync();
      }
}
