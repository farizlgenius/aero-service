using System;
using Aero.Domain.Entities;
using Aero.Domain.Interface;
using Aero.Infrastructure.Data;
using Aero.Infrastructure.Mapper;
using Microsoft.EntityFrameworkCore;

namespace Aero.Infrastructure.Repositories;

public class CfmtRepository(AppDbContext context) : ICfmtRepository
{
      public async Task<int> AddAsync(CardFormat data)
      {
           var en = CardFormatMapper.ToEf(data);
           await context.card_format.AddAsync(en);
           return await context.SaveChangesAsync();
      }

      public async Task<int> DeleteByComponentIdAsync(short component)
      {
            var en = await context.card_format
            .Where(x => x.component_id == component)
            .OrderBy(x => x.component_id)
            .FirstOrDefaultAsync();

            if(en is null) return 0;

            context.card_format.Remove(en);
            return await context.SaveChangesAsync();
      }

      public async Task<int> UpdateAsync(CardFormat newData)
      {
            var en = await context.card_format
            .Where(x => x.component_id == newData.ComponentId)
            .OrderBy(x => x.component_id)
            .FirstOrDefaultAsync();

            if(en is null) return 0;

            CardFormatMapper.Update(en,newData);

            context.card_format.Update(en);
            return await context.SaveChangesAsync();
      }
}
