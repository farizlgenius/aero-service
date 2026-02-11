using System;
using Aero.Application.DTOs;
using Aero.Application.Interfaces;
using Aero.Domain.Entities;
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

      public async Task<IEnumerable<OperatorDto>> GetByLocationIdAsync(short locationId)
      {
        var dto = await context.@operator
            .AsNoTracking()
            .Where(x => x.operator_locations.Any(x => x.location_id == locationId || x.location_id == 1))
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

      public async Task<short> GetLowestUnassignedNumberAsync(int max, string mac)
      {
        if (max <= 0) return -1;

        var query = context.@operator
            .AsNoTracking()
            .Select(x => x.component_id);

        // Handle empty table case quickly
        var hasAny = await query.AnyAsync();
        if (!hasAny)
            return 1; // start at 1 if table is empty

        // Load all numbers into memory (only the column, so it's lightweight)
        var numbers = await query.Distinct().OrderBy(x => x).ToListAsync();

        short expected = 1;
        foreach (var num in numbers)
        {
            if (num != expected)
                return expected; // found the lowest missing number
            expected++;
        }

        // If none missing in sequence, return next number
        if (expected > max) return -1;
        return expected;
    }

      public async Task<string> GetPasswordByUsername(string username)
      {
            return await context.@operator.AsNoTracking().Where(x => x.user_name.Equals(username)).Select(x => x.password).FirstOrDefaultAsync() ?? "";
      }

    public async Task<Pagination<OperatorDto>> GetPaginationAsync(PaginationParamsWithFilter param,short location)
    {
        var query = context.@operator.AsNoTracking().AsQueryable();


        if (!string.IsNullOrWhiteSpace(param.Search))
        {
            if (!string.IsNullOrWhiteSpace(param.Search))
            {
                var search = param.Search.Trim();

                if (context.Database.IsNpgsql())
                {
                    var pattern = $"%{search}%";

                    query = query.Where(x =>
                        EF.Functions.ILike(x.user_id, pattern) ||
                        EF.Functions.ILike(x.user_name, pattern) ||
                        EF.Functions.ILike(x.email, pattern) ||
                        EF.Functions.ILike(x.title, pattern) ||
                        EF.Functions.ILike(x.first_name, pattern) ||
                        EF.Functions.ILike(x.middle_name, pattern) ||
                        EF.Functions.ILike(x.last_name, pattern) ||
                        EF.Functions.ILike(x.phone, pattern) 

                    );
                }
                else // SQL Server
                {
                    query = query.Where(x =>
                        x.user_id.Contains(search) ||
                        x.user_name.Contains(search) ||
                        x.email.Contains(search) ||
                        x.title.Contains(search) ||
                        x.first_name.Contains(search) ||
                        x.middle_name.Contains(search) ||
                        x.last_name.Contains(search) ||
                        x.phone.Contains(search) 
                    );
                }
            }
        }

        query = query.Where(x => x.operator_locations.Any(x => x.location_id == location || x.location_id == 1));

        if (param.StartDate != null)
        {
            var startUtc = DateTime.SpecifyKind(param.StartDate.Value, DateTimeKind.Utc);
            query = query.Where(x => x.created_date >= startUtc);
        }

        if (param.EndDate != null)
        {
            var endUtc = DateTime.SpecifyKind(param.EndDate.Value, DateTimeKind.Utc);
            query = query.Where(x => x.created_date <= endUtc);
        }

        var count = await query.CountAsync();


        var data = await query
            .AsNoTracking()
            .OrderByDescending(t => t.created_date)
            .Skip((param.PageNumber - 1) * param.PageSize)
            .Take(param.PageSize)
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
            .ToListAsync();


        return new Pagination<OperatorDto>
        {
            Data = data,
            Page = new PaginationData
            {
                TotalCount = count,
                PageNumber = param.PageNumber,
                PageSize = param.PageSize,
                TotalPage = (int)Math.Ceiling(count / (double)param.PageSize)
            }
        };
    }

    public async Task<bool> IsAnyByComponentId(short component)
      {
        return await context.@operator.AnyAsync(x => x.component_id == component);
      }

      public async Task<bool> IsAnyByUsernameAsync(string name)
      {
            return await context.@operator.AnyAsync(x => x.user_name.Equals(name));
      }
}
