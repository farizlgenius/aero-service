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

      public async Task<int> AddCreateAsync(CreateUpdateAccessLevel domain)
      {
            await context.accesslevel.AddAsync(Aero.Infrastructure.Mapper.AccessLevelMapper.ToEf(domain));
            return await context.SaveChangesAsync();
      }

      public async Task<int> DeleteByComponentIdAsync(short component)
      {
            var en = await context.accesslevel
            .Where(x => x.component_id == component)
            .OrderBy(x => x.component_id)
            .FirstOrDefaultAsync();

            if(en is null) return 0;

            context.accesslevel.Remove(en);
            return await context.SaveChangesAsync();
      }

      public async Task<int> UpdateAsync(AccessLevel newData)
      {
            throw new NotImplementedException();
      }

      public async Task<int> UpdateCreateAsync(CreateUpdateAccessLevel domain)
      {
            var en = await context.accesslevel
            .Where(x => x.component_id == domain.ComponentId)
            .OrderBy(x => x.component_id)
            .FirstOrDefaultAsync();

            if(en is null) return 0;

            var e = await context.accesslevel_door_timezone
            .Where(x => x.accesslevel_id == domain.ComponentId)
            .ToArrayAsync();

            context.accesslevel_door_timezone.RemoveRange(e);

            Aero.Infrastructure.Mapper.AccessLevelMapper.Update(domain,en);

            return await context.SaveChangesAsync();
      }
}
