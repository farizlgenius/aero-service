using System;
using Aero.Domain.Entities;
using Aero.Domain.Interface;
using Aero.Infrastructure.Data;
using Aero.Infrastructure.Mapper;
using Microsoft.EntityFrameworkCore;

namespace Aero.Infrastructure.Repositories;

public sealed class ProcedureRepository(AppDbContext context) : IProcedureRepository
{
      public async Task<int> AddAsync(Procedure data)
      {
            var en = ProcedureMapper.ToEf(data);
            await context.procedure.AddAsync(en);
            return await context.SaveChangesAsync();
      }

      public async Task<int> DeleteByComponentIdAsync(short component)
      {
            var en = await context.procedure
            .Where(x => x.component_id == component)
            .OrderBy(x => x.component_id)
            .FirstOrDefaultAsync();

            if(en is null) return 0;

            context.procedure.Remove(en);
            return await context.SaveChangesAsync();
      }

      public async Task<int> UpdateAsync(Procedure newData)
      {
            var en = await context.procedure
            .Include(x => x.actions)
            .Where(x => x.component_id == newData.ComponentId)
            .OrderBy(x => x.component_id)
            .FirstOrDefaultAsync();

            if(en is null) return 0;

            context.action.RemoveRange(en.actions);

            ProcedureMapper.Update(newData,en);

            context.procedure.Update(en);
            return await context.SaveChangesAsync();


      }
}
