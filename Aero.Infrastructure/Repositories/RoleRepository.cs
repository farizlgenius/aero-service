using System;
using Aero.Application.Helpers;
using Aero.Domain.Entities;
using Aero.Domain.Interface;
using Aero.Infrastructure.Data;
using Aero.Infrastructure.Mapper;
using Microsoft.EntityFrameworkCore;

namespace Aero.Infrastructure.Repositories;

public class RoleRepository(AppDbContext context) : IRoleRepository
{
      public async Task<int> AddAsync(Role data)
      {
            var en = RoleMapper.ToEf(data);
            await context.role.AddAsync(en);
            return await context.SaveChangesAsync();
      }

      public async Task<int> DeleteByComponentIdAsync(short component)
      {
            var en = await context.role
            .Where(x => x.component_id == component)
            .OrderBy(x => x.component_id)
            .FirstOrDefaultAsync();

            if(en is null) return 0;

            context.role.Remove(en);
            return await context.SaveChangesAsync();


      }

      public async Task<int> UpdateAsync(Role newData)
      {
            var en = await context.role
            .Include(x => x.feature_roles)
            .OrderBy(x => x.component_id)
            .Where(x => x.component_id == newData.ComponentId)
            .FirstOrDefaultAsync();

            if(en is null) return 0;

            context.feature_role.RemoveRange(en.feature_roles);

            RoleMapper.Update(newData,en);

            context.role.Update(en);
            return await context.SaveChangesAsync();
      }
}
