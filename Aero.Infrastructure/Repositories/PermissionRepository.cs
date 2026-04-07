using System;
using Aero.Application.DTOs;
using Aero.Application.Interfaces;
using Aero.Domain.Entities;
using Aero.Infrastructure.Persistences;
using Microsoft.EntityFrameworkCore;

namespace Aero.Infrastructure.Repositories;

public sealed class PermissionRepository(AppDbContext context) : IPermissionRepository
{
      public async Task<HashSet<PermissionDto>> GetPermissionsFromRoleIdAsync(int roleId)
      {
           var data = await context.permissions
           .AsNoTracking()
           .Where(x => x.role_id == roleId)
           .Select(x => new PermissionDto(
            x.id,
            x.source.name,
            x.is_allow,
            x.is_create,
            x.is_modify,     
            x.is_delete,
            x.is_action
           ))
           .ToArrayAsync();

            return data.ToHashSet();

      }

      public Task<int> AddAsync(Permission data)
      {
            throw new NotImplementedException();
      }

      public Task<int> DeleteByIdAsync(int id)
      {
            throw new NotImplementedException();
      }

      public async Task<IEnumerable<PermissionDto>> GetAsync()
      {
            var res = await context.permissions
            .AsNoTracking()
            .OrderBy(x => x.id)
            .Select(f => new PermissionDto(
                  f.source_id,
                  f.source.name,
                  false,
                  false,
                  false,
                  false,
                  false
                  ))
            .ToArrayAsync();

            return res;

      }

      public async Task<PermissionDto> GetByIdAsync(int id)
      {
            var res = await context.permissions
            .AsNoTracking()
            .Where(x => x.id == id)
            .OrderBy(x => x.id)
            .Select(f => new PermissionDto(
                  f.id,
                  f.source.name,
                  false,
                  false,
                  false,
                  false,
                  false
                  ))
            .FirstOrDefaultAsync();

            return res;
      }

      public Task<IEnumerable<PermissionDto>> GetByLocationIdAsync(short locationId)
      {
            throw new NotImplementedException();
      }

      public Task<IEnumerable<PermissionDto>> GetByLocationIdAsync(int locationId)
      {
            throw new NotImplementedException();
      }

     

      public async Task<PermissionDto> GetFeatureByRoleIdAndFeatureIdAsync(short RoleId, short FeatureId)
      {
            var f = await context.permissions
                 .AsNoTracking()
                 .Where(f => f.role_id == RoleId && f.id == FeatureId)
                 .Select(f => new PermissionDto(
                  f.id,
                  f.source.name,
                  f.is_allow,
                  f.is_create,
                  f.is_modify,
                  f.is_delete,
                  f.is_action
                 ))
                 .FirstOrDefaultAsync();


            return f;
      }

      public Task<short> GetLowestUnassignedNumberAsync(int max,string mac)
      {
            throw new NotImplementedException();
      }

    public Task<Pagination<PermissionDto>> GetPaginationAsync(PaginationParamsWithFilter param, int location)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> IsAnyByIdAsync(int id)
      {
            return await context.permissions.AnyAsync(x => x.id == id);
      }

      public Task<bool> IsAnyByNameAsync(string name)
      {
            throw new NotImplementedException();
      }

      public Task<int> UpdateAsync(Permission data)
      {
            throw new NotImplementedException();
      }

      public async Task<bool> IsPermissionAllowAsync(int roleId, string source, string action)
      {
            return await context.permissions
            .AsNoTracking()
            .AnyAsync(x => x.role_id == roleId && x.source.name == source && 
            ((action == "read" && x.is_allow) ||
             (action == "create" && x.is_create) ||
             (action == "modify" && x.is_modify) ||
             (action == "delete" && x.is_delete) ||
             (action == "action" && x.is_action)));
      }
}
