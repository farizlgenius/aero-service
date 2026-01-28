using System;
using Aero.Domain.Entities;
using Aero.Domain.Interface;
using Aero.Infrastructure.Data;
using Aero.Infrastructure.Mapper;
using Microsoft.EntityFrameworkCore;

namespace Aero.Infrastructure.Repositories;

public class CpRepository(AppDbContext context) : ICpRepository
{
      public async Task<int> AddAsync(ControlPoint data)
      {
            var en = ControlPointMapper.ToEf(data);
            await context.control_point.AddAsync(en);
            return await context.SaveChangesAsync();
      }

      public async Task<int> DeleteByComponentIdAsync(short component)
      {
            var en = await context.control_point
            .Where(x => x.component_id == component)
            .FirstOrDefaultAsync();

            if(en is null) return 0;

            context.control_point.Remove(en);
            return await context.SaveChangesAsync();
      }

      public async Task<int> UpdateAsync(ControlPoint newData)
      {
            var en = await context.control_point
            .Where(x => x.component_id == newData.ComponentId)
            .FirstOrDefaultAsync();

            if(en is null) return 0;

            ControlPointMapper.Update(en,newData);

            context.control_point.Update(en);
            return await context.SaveChangesAsync();
      }
}
