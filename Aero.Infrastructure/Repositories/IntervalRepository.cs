using System;
using System.Security.Cryptography.X509Certificates;
using Aero.Domain.Entities;
using Aero.Domain.Interface;
using Aero.Infrastructure.Data;
using Aero.Infrastructure.Mapper;
using Microsoft.EntityFrameworkCore;

namespace Aero.Infrastructure.Repositories;

public class IntervalRepository(AppDbContext context) : IIntervalRepository
{
      public async Task<int> AddAsync(Interval data)
      {
           await context.interval.AddAsync(IntervalMapper.ToEf(data)); 
           return await context.SaveChangesAsync();
      }

      public async Task<int> DeleteByComponentIdAsync(short component)
      {
            var en = await context.interval
            .Include(x => x.timezone_intervals)
            .Where(x => x.component_id == component)
            .OrderBy(x => x.component_id)
            .FirstOrDefaultAsync();

            if(en is null) return 0;

            context.timezone_interval.RemoveRange(en.timezone_intervals);
            context.interval.Remove(en);
            return await context.SaveChangesAsync();
      }

      public async Task<int> UpdateAsync(Interval newData)
      {
            context.interval.Update(IntervalMapper.ToEf(newData));
            return await context.SaveChangesAsync();
            
      }
}
