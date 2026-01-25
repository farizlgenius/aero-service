using System;
using Aero.Domain.Entities;
using Aero.Domain.Interface;
using Aero.Infrastructure.Data;
using Aero.Infrastructure.Mapper;
using Microsoft.EntityFrameworkCore;

namespace Aero.Infrastructure.Repositories;

public class HolRepository(AppDbContext context) : IHolRepository
{
      public async Task<int> AddAsync(Holiday data)
      {
            await context.holiday.AddAsync(HolidayMapper.ToEf(data));
            return await context.SaveChangesAsync();
      }

      public async Task<int> DeleteByComponentIdAsync(short component)
      {
            var en = await context.holiday
            .Where(x => x.component_id == component)
            .OrderBy(x => x.component_id)
            .FirstOrDefaultAsync();

            if(en is null) return 0;

            context.holiday.Remove(en);
            return await context.SaveChangesAsync();
      }

      public async Task<Holiday> GetByComponentIdAsync(short component)
      {
            var res = await context.holiday.AsNoTracking()
            .Where(x => x.component_id == component)
            .Select(p => new Holiday 
            {
                // Base
                Uuid = p.uuid,
                LocationId = p.location_id,
                IsActive = p.is_active,

                // extend_desc
                Day = p.day,
                Month = p.month,
                Year = p.year,
                Extend = p.extend,
                TypeMask = p.type_mask

            })
            .OrderBy(x => x.ComponentId)
            .FirstOrDefaultAsync();

            return res;
      }

      public async Task<int> RemoveAllAsync()
      {
            var en = await context.holiday.ToArrayAsync();
            context.holiday.RemoveRange(en);
            return await context.SaveChangesAsync();
      }

      public async Task<int> UpdateAsync(Holiday newData)
      {
            var en = await context.holiday
            .Where(x => x.component_id == newData.ComponentId)
            .OrderBy(x => x.component_id)
            .FirstOrDefaultAsync();

            if(en is null) return 0; 

            HolidayMapper.Update(en,newData);

            context.holiday.Update(en);
            return await context.SaveChangesAsync();
      }
}
