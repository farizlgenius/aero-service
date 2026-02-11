using System;
using Aero.Application.DTOs;
using Aero.Application.Interfaces;
using Aero.Domain.Entities;
using Aero.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Aero.Infrastructure.Repositories;

public class QRoleRepository(AppDbContext context) : IQRoleRepository
{
      public async Task<IEnumerable<RoleDto>> GetAsync()
      {
            var dtos = await context.role
            .AsNoTracking()
            .Select(x => new RoleDto
            {
                  ComponentId = x.component_id,
                  Name = x.name,
                  Features = x.feature_roles != null && x.feature_roles.Count > 0 ? x.feature_roles.Select(x => new FeatureDto
                  {
                        ComponentId = x.feature.component_id,
                        Name = x.feature.name,
                        Path = x.feature.path,
                        SubItems = x.feature.sub_feature.Select(s => new SubFeatureDto
                        {
                              Name = s.name,
                              Path = s.path
                        }).ToList(),
                        IsAction = x.is_action,
                        IsAllow = x.is_allow,
                        IsCreate = x.is_create,
                        IsModify = x.is_modify,
                        IsDelete = x.is_delete,

                  }).ToList() : new List<FeatureDto>()
            }).ToArrayAsync();

            return dtos;
      }

      public async Task<RoleDto> GetByComponentIdAsync(short componentId)
      {
            var dtos = await context.role
            .AsNoTracking()
            .Where(x => x.component_id == componentId)
            .OrderBy(x => x.component_id)
            .Select(x => new RoleDto
            {
                  ComponentId = x.component_id,
                  Name = x.name,
                  Features = x.feature_roles != null && x.feature_roles.Count > 0 ? x.feature_roles.Select(x => new FeatureDto
                  {
                        ComponentId = x.feature.component_id,
                        Name = x.feature.name,
                        Path = x.feature.path,
                        SubItems = x.feature.sub_feature.Select(s => new SubFeatureDto
                        {
                              Name = s.name,
                              Path = s.path
                        }).ToList(),
                        IsAction = x.is_action,
                        IsAllow = x.is_allow,
                        IsCreate = x.is_create,
                        IsModify = x.is_modify,
                        IsDelete = x.is_delete,

                  }).ToList() : new List<FeatureDto>()
            }).FirstOrDefaultAsync();

            return dtos;
      }

      public Task<IEnumerable<RoleDto>> GetByLocationIdAsync(short locationId)
      {
            throw new NotImplementedException();
      }

      public async Task<short> GetLowestUnassignedNumberAsync(int max, string mac)
      {
            if (max <= 0) return -1;

            var query = context.role
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

    public async Task<Pagination<RoleDto>> GetPaginationAsync(PaginationParamsWithFilter param, short location)
    {

        var query = context.role.AsNoTracking().AsQueryable();


        if (!string.IsNullOrWhiteSpace(param.Search))
        {
            if (!string.IsNullOrWhiteSpace(param.Search))
            {
                var search = param.Search.Trim();

                if (context.Database.IsNpgsql())
                {
                    var pattern = $"%{search}%";

                    query = query.Where(x =>
                        EF.Functions.ILike(x.name, pattern) 

                    );
                }
                else // SQL Server
                {
                    query = query.Where(x =>
                        x.name.Contains(search) 
                    );
                }
            }
        }

        query = query.Where(x => x.location_id == location || x.location_id == 1);

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
           .Select(x => new RoleDto
           {
               ComponentId = x.component_id,
               Name = x.name,
               Features = x.feature_roles != null && x.feature_roles.Count > 0 ? x.feature_roles.Select(x => new FeatureDto
               {
                   ComponentId = x.feature.component_id,
                   Name = x.feature.name,
                   Path = x.feature.path,
                   SubItems = x.feature.sub_feature.Select(s => new SubFeatureDto
                   {
                       Name = s.name,
                       Path = s.path
                   }).ToList(),
                   IsAction = x.is_action,
                   IsAllow = x.is_allow,
                   IsCreate = x.is_create,
                   IsModify = x.is_modify,
                   IsDelete = x.is_delete,

               }).ToList() : new List<FeatureDto>()
           })
            .ToListAsync();


        return new Pagination<RoleDto>
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
            return await context.role.AnyAsync(x => x.component_id == component);
      }

      public async Task<bool> IsAnyByNameAsync(string name)
      {
            return await context.role.AnyAsync(x => x.name.Equals(name));
      }

      public async Task<bool> IsAnyReferenceByComponentIdAsync(short component)
      {
            return await context.role.AnyAsync(x => x.component_id == component && x.operators.Any());
      }
}
