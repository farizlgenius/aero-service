using System;
using Aero.Application.DTOs;
using Aero.Application.Interfaces;
using Aero.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Aero.Infrastructure.Repositories;

public class QOperatorRepository(AppDbContext context) : IQOperatorRepository
{
      public async Task<IEnumerable<OperatorDto>> GetAsync()
      {
             var dto = await context.@operator
                .AsNoTracking()
                .Select(x => new OperatorDto
                {
                    LocationIds = x.operator_locations.Select(x => x.location.component_id).ToList(),
                    IsActive = x.is_active,

                    // extend_desc 
                    ComponentId = x.component_id,
                    Username = x.user_name,
                    Email = x.email,
                    Title = x.title,
                    FirstName = x.first_name,
                    MiddleName = x.middle_name,
                    LastName = x.last_name,
                    Phone = x.phone,
                    Image = x.image_path,
                    RoleId = x.role_id,
                })
                .ToArrayAsync();

            return dto;
      }

      public Task<OperatorDto> GetByComponentIdAsync(short componentId)
      {
            throw new NotImplementedException();
      }

      public Task<IEnumerable<OperatorDto>> GetByLocationIdAsync(short locationId)
      {
            throw new NotImplementedException();
      }

      public async Task<OperatorDto> GetByUsernameAsync(string username)
      {
           var dto = await context.@operator
                .AsNoTracking()
                .Where(o => o.user_name.Equals(username))
                .Select(x => new OperatorDto
                {
                    LocationIds = x.operator_locations.Select(x => x.location.component_id).ToList(),
                    IsActive = x.is_active,

                    // extend_desc 
                    ComponentId = x.component_id,
                    Username = x.user_name,
                    Email = x.email,
                    Title = x.title,
                    FirstName = x.first_name,
                    MiddleName = x.middle_name,
                    LastName = x.last_name,
                    Phone = x.phone,
                    Image = x.image_path,
                    RoleId = x.role_id,
                })
                .FirstOrDefaultAsync();
            
            return dto;

      }

      public Task<short> GetLowestUnassignedNumberAsync(int max, string mac)
      {
            throw new NotImplementedException();
      }

      public async Task<string> GetPasswordByUsername(string username)
      {
            return await context.@operator.AsNoTracking().Where(x => x.user_name.Equals(username)).Select(x => x.password).FirstOrDefaultAsync() ?? "";
      }

      public Task<bool> IsAnyByComponentId(short component)
      {
            throw new NotImplementedException();
      }

      public async Task<bool> IsAnyByUsernameAsync(string name)
      {
            return await context.@operator.AnyAsync(x => x.user_name.Equals(name));
      }
}
