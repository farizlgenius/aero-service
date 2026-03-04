using System;
using Aero.Application.DTOs;
using Aero.Application.Interfaces;
using Aero.Domain.Entities;
using Aero.Infrastructure.Persistences;
using Microsoft.EntityFrameworkCore;

namespace Aero.Infrastructure.Repositories;

public class FeatureRepository(AppDbContext context) : IFeatureRepository
{
      public Task<int> AddAsync(Feature data)
      {
            throw new NotImplementedException();
      }

      public Task<int> DeleteByIdAsync(int id)
      {
            throw new NotImplementedException();
      }

      public async Task<IEnumerable<FeatureDto>> GetAsync()
      {
            var res = await context.feature
            .AsNoTracking()
            .OrderBy(x => x.id)
            .Select(f => new FeatureDto(
                  f.id,
                  f.name,
                  f.path,
                  f.sub_feature.Select(s => new SubFeatureDto(
                        s.name,
                        s.path
                  )).ToList(),
                  false,
                  false,
                  false,
                  false,
                  false
                  ))
            .ToArrayAsync();

            return res;

      }

      public async Task<FeatureDto> GetByIdAsync(int id)
      {
            var res = await context.feature
            .AsNoTracking()
            .Where(x => x.id == id)
            .OrderBy(x => x.id)
            .Select(f => new FeatureDto(
                  f.id,
                  f.name,
                  f.path,
                  f.sub_feature.Select(s => new SubFeatureDto(
                        s.name,
                        s.path
                  )).ToList(),
                  false,
                  false,
                  false,
                  false,
                  false
                  ))
            .FirstOrDefaultAsync();

            return res;
      }

      public Task<IEnumerable<FeatureDto>> GetByLocationIdAsync(short locationId)
      {
            throw new NotImplementedException();
      }

      public Task<IEnumerable<FeatureDto>> GetByLocationIdAsync(int locationId)
      {
            throw new NotImplementedException();
      }

      public async Task<IEnumerable<FeatureDto>> GetFeatureByRoleAsync(short RoleId)
      {
            var data = await context.feature_role
                 .AsNoTracking()
                 .Where(f => f.role_id == RoleId)
                 .Select(f => new {
                  f.id,
                  f.feature.name,
                  f.feature.path,
                  sub = f.feature.sub_feature.Select(s => new SubFeatureDto(s.name,s.path)),
                  f.is_allow,
                  f.is_create,
                  f.is_modify,
                  f.is_delete,
                  f.is_action
                  })
                 .ToArrayAsync();

            var dtos = data.Select(f => new FeatureDto(
                  f.id,
                  f.name,
                  f.path,
                  f.sub.Select(s => new SubFeatureDto(
                        s.Name,
                        s.Path
                  )).ToList(),
                  f.is_allow,
                  f.is_create,
                  f.is_modify,
                  f.is_delete,
                  f.is_action
            )).ToList();

            return dtos;
      }

      public async Task<FeatureDto> GetFeatureByRoleIdAndFeatureIdAsync(short RoleId, short FeatureId)
      {
            var f = await context.feature_role
                 .AsNoTracking()
                 .Where(f => f.role_id == RoleId && f.feature_id == FeatureId)
                 .Select(f => new {
                  f.id,
                  f.feature.name,
                  f.feature.path,
                  sub = f.feature.sub_feature.Select(s => new SubFeatureDto(s.name,s.path)),
                  f.is_allow,
                  f.is_create,
                  f.is_modify,
                  f.is_delete,
                  f.is_action
                  })
                 .FirstOrDefaultAsync();

            var dto = new FeatureDto(
                  f.id,
                  f.name,
                  f.path,
                  f.sub.Select(s => new SubFeatureDto(
                        s.Name,
                        s.Path
                  )).ToList(),
                  f.is_allow,
                  f.is_create,
                  f.is_modify,
                  f.is_delete,
                  f.is_action
            );

            return dto;
      }

      public Task<short> GetLowestUnassignedNumberAsync(int max,string mac)
      {
            throw new NotImplementedException();
      }

    public Task<Pagination<FeatureDto>> GetPaginationAsync(PaginationParamsWithFilter param, int location)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> IsAnyByIdAsync(int id)
      {
            return await context.feature.AnyAsync(x => x.id == id);
      }

      public Task<bool> IsAnyByNameAsync(string name)
      {
            throw new NotImplementedException();
      }

      public Task<int> UpdateAsync(Feature data)
      {
            throw new NotImplementedException();
      }
}
