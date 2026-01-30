using System;
using Aero.Application.DTOs;
using Aero.Application.Interfaces;
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
