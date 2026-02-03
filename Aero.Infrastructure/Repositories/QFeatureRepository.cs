using System;
using Aero.Application.DTOs;
using Aero.Application.Interfaces;
using Aero.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Aero.Infrastructure.Repositories;

public class QFeatureRepository(AppDbContext context) : IQFeatureRepository
{
      public async Task<IEnumerable<FeatureDto>> GetAsync()
      {
            var res = await context.feature
            .AsNoTracking()
            .OrderBy(x => x.component_id)
            .Select(fn => new FeatureDto
            {
                  ComponentId = fn.component_id,
                  Name = fn.name,
                  Path = fn.path,
                  SubItems = fn.sub_feature == null || fn.sub_feature.Count == 0 ? new List<SubFeatureDto>() : fn.sub_feature.Select(s => new SubFeatureDto
                  {
                        Path = s.path,
                        Name = s.name
                  }).ToList(),
                  IsAllow = false,
                  IsCreate = false,
                  IsModify = false,
                  IsDelete = false,
                  IsAction = false
            }).ToArrayAsync();

            return res;

      }

      public async Task<FeatureDto> GetByComponentIdAsync(short componentId)
      {
            var res = await context.feature
            .AsNoTracking()
            .Where(x => x.component_id == componentId)
            .OrderBy(x => x.component_id)
            .Select(fn => new FeatureDto
            {
                  ComponentId = fn.component_id,
                  Name = fn.name,
                  Path = fn.path,
                  SubItems = fn.sub_feature == null || fn.sub_feature.Count == 0 ? new List<SubFeatureDto>() : fn.sub_feature.Select(s => new SubFeatureDto
                  {
                        Path = s.path,
                        Name = s.name
                  }).ToList(),
                  IsAllow = false,
                  IsCreate = false,
                  IsModify = false,
                  IsDelete = false,
                  IsAction = false
            }).FirstOrDefaultAsync();

            return res;
      }

      public Task<IEnumerable<FeatureDto>> GetByLocationIdAsync(short locationId)
      {
            throw new NotImplementedException();
      }

      public async Task<IEnumerable<FeatureDto>> GetFeatureByRoleAsync(short RoleId)
      {
            var dtos = await context.feature_role
                 .AsNoTracking()
                 .Where(f => f.role_id == RoleId)
                 .Select(fn => new FeatureDto
                 {
                       ComponentId = fn.feature.component_id,
                       Name = fn.feature.name,
                       Path = fn.feature.path,
                       SubItems = fn.feature.sub_feature == null || fn.feature.sub_feature.Count == 0 ? new List<SubFeatureDto>() : fn.feature.sub_feature.Select(s => new SubFeatureDto
                       {
                             Path = s.path,
                             Name = s.name
                       }).ToList(),
                       IsAllow = fn.is_allow,
                       IsCreate = fn.is_create,
                       IsModify = fn.is_modify,
                       IsDelete = fn.is_delete,
                       IsAction = fn.is_action
                 })
                 .ToArrayAsync();

            return dtos;
      }

      public async Task<FeatureDto> GetFeatureByRoleIdAndFeatureIdAsync(short RoleId, short FeatureId)
      {
            var dtos = await context.feature_role
                 .AsNoTracking()
                 .Where(f => f.role_id == RoleId && f.feature_id == FeatureId)
                 .Select(fn => new FeatureDto
                 {
                       ComponentId = fn.feature.component_id,
                       Name = fn.feature.name,
                       Path = fn.feature.path,
                       SubItems = fn.feature.sub_feature == null || fn.feature.sub_feature.Count == 0 ? new List<SubFeatureDto>() : fn.feature.sub_feature.Select(s => new SubFeatureDto
                       {
                             Path = s.path,
                             Name = s.name
                       }).ToList(),
                       IsAllow = fn.is_allow,
                       IsCreate = fn.is_create,
                       IsModify = fn.is_modify,
                       IsDelete = fn.is_delete,
                       IsAction = fn.is_action
                 })
                 .FirstOrDefaultAsync();

            return dtos;
      }

      public Task<short> GetLowestUnassignedNumberAsync(int max,string mac)
      {
            throw new NotImplementedException();
      }

      public async Task<bool> IsAnyByComponentId(short component)
      {
            return await context.feature.AnyAsync(x => x.component_id == component);
      }
}
