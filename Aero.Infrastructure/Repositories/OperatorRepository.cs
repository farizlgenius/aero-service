using System;
using Aero.Domain.Entities;
using Aero.Domain.Interface;
using Aero.Infrastructure.Data;
using Aero.Infrastructure.Mapper;
using Microsoft.EntityFrameworkCore;

namespace Aero.Infrastructure.Repositories;

public class OperatorRepository(AppDbContext context) : IOperatorRepository
{
      public async Task<int> AddAsync(Operator data)
      {
            throw new NotImplementedException();
      }

      public async Task<int> AddAsync(CreateOperator dto)
      {
            var en = OperatorMapper.ToEf(dto);
            await context.@operator.AddAsync(en);
            return await context.SaveChangesAsync();
      }

      public async Task<int> DeleteByComponentIdAsync(short component)
      {
           var en = await context.@operator
           .Where(x => x.component_id == component)
           .OrderBy(x => x.component_id)
           .FirstOrDefaultAsync();

           if(en is null) return 0;

           context.@operator.Remove(en);
           return await context.SaveChangesAsync();
      }

      public Task<int> UpdateAsync(Operator newData)
      {
            throw new NotImplementedException();
      }

      public async Task<int> UpdateAsync(CreateOperator data)
      {
            var en = await context.@operator
            .Include(x => x.operator_locations)
            .Where(x => x.user_name.Equals(data.Username))
            .OrderBy(x => x.component_id)
            .FirstOrDefaultAsync();

            if(en is null) return 0;

            context.operator_location.RemoveRange(en.operator_locations);

            context.@operator.Update(en);
            return await context.SaveChangesAsync();

      }

      public async Task<int> UpdatePasswordAsync(string username, string password)
      {
            var en = await context.@operator
            .Where(x => x.user_name.Equals(username))
            .FirstOrDefaultAsync();

            if(en is null) return 0;

            en.password = password;

            context.@operator.Update(en);
            return await context.SaveChangesAsync();
      }
}
