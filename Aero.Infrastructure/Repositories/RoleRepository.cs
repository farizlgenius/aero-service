using Aero.Application.DTOs;
using Aero.Application.Helpers;
using Aero.Domain.Entities;
using Aero.Domain.Interface;
using Aero.Infrastructure.Persistences;
using Aero.Infrastructure.Mapper;
using Microsoft.EntityFrameworkCore;
using System;
using Aero.Application.Interface;

namespace Aero.Infrastructure.Repositories;

public class RoleRepository(AppDbContext context) : IRoleRepository
{
      public async Task<int> AddAsync(Aero.Domain.Entities.Role data)
      {
            var en = new Aero.Infrastructure.Persistences.Entities.Role(data);
            await context.role.AddAsync(en);
            var record =  await context.SaveChangesAsync();
        if (record <= 0) return -1;
        return en.id;
      }

      public async Task<int> DeleteByIdAsync(int id)
      {
            var en = await context.role
            .Where(x => x.id == id)
            .OrderBy(x => x.id)
            .FirstOrDefaultAsync();

            if(en is null) return 0;

            context.role.Remove(en);
            return await context.SaveChangesAsync();


      }

      public async Task<int> UpdateAsync(Aero.Domain.Entities.Role data)
      {
            var en = await context.role
            .Include(x => x.feature_roles)
            .OrderBy(x => x.id)
            .Where(x => x.id == data.Id)
            .FirstOrDefaultAsync();

            if(en is null) return 0;

            context.feature_role.RemoveRange(en.feature_roles);

        en.Update(data);

            context.role.Update(en);
            return await context.SaveChangesAsync();
      }

    public async Task<IEnumerable<RoleDto>> GetAsync()
    {
        var data = await context.role
        .AsNoTracking()
        .Select(r => new 
        { 
            r.id, 
            r.driver_id, 
            r.name, 
            features = r.feature_roles.Select(fr => new
            {
                fr.feature,
                fr.is_allow,
                fr.is_create,
                fr.is_modify,
                fr.is_delete,
                fr.is_action

            }), 
            r.location_id, 
            r.is_active 
        })
        .ToArrayAsync();

        var res = data.Select(d => new RoleDto(
            d.id,
            d.driver_id,
            d.name,
            d.features.Select(fr => new FeatureDto(fr.feature.id,fr.feature.name,fr.feature.path,fr.feature.sub_feature.Select(sf => new SubFeatureDto(sf.name,sf.path)).ToList(),fr.is_allow,fr.is_create,fr.is_modify,fr.is_delete,fr.is_action)).ToList(),
            d.location_id,
            d.is_active))
            .ToList();

        return res;
    }

    public async Task<RoleDto> GetByIdAsync(int id)
    {
        var d = await context.role
        .AsNoTracking()
        .Where(x => x.id == id)
        .OrderBy(x => x.id)
        .Select(r => new
        {
            r.id,
            r.driver_id,
            r.name,
            features = r.feature_roles.Select(fr => new
            {
                fr.feature,
                fr.is_allow,
                fr.is_create,
                fr.is_modify,
                fr.is_delete,
                fr.is_action

            }),
            r.location_id,
            r.is_active
        }).FirstOrDefaultAsync();

        var res = new RoleDto(
           d.id,
           d.driver_id,
           d.name,
           d.features.Select(fr => new FeatureDto(fr.feature.id, fr.feature.name, fr.feature.path, fr.feature.sub_feature.Select(sf => new SubFeatureDto(sf.name, sf.path)).ToList(), fr.is_allow, fr.is_create, fr.is_modify, fr.is_delete, fr.is_action)).ToList(),
           d.location_id,
           d.is_active
           );

        return res;
    }

    public async Task<IEnumerable<RoleDto>> GetByLocationIdAsync(int locationId)
    {
        var data = await context.role
       .AsNoTracking()
       .Where(x => x.location_id == locationId || x.location_id == 1)
       .Select(r => new
       {
           r.id,
           r.driver_id,
           r.name,
           features = r.feature_roles.Select(fr => new
           {
               fr.feature,
               fr.is_allow,
               fr.is_create,
               fr.is_modify,
               fr.is_delete,
               fr.is_action

           }),
           r.location_id,
           r.is_active
       })
       .ToArrayAsync();

        var res = data.Select(d => new RoleDto(
            d.id,
            d.driver_id,
            d.name,
            d.features.Select(fr => new FeatureDto(fr.feature.id, fr.feature.name, fr.feature.path, fr.feature.sub_feature.Select(sf => new SubFeatureDto(sf.name, sf.path)).ToList(), fr.is_allow, fr.is_create, fr.is_modify, fr.is_delete, fr.is_action)).ToList(),
            d.location_id,
            d.is_active))
            .ToList();

        return res;
    }

    public async Task<short> GetLowestUnassignedNumberAsync(int max)
    {
        if (max <= 0) return -1;

        var query = context.role
            .AsNoTracking()
            .Select(x => x.driver_id);

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

    public async Task<short> GetLowestUnassignedNumberAsync()
    {

        var query = context.role
            .AsNoTracking()
            .Select(x => x.driver_id);

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
        return expected;
    }

    public async Task<Pagination<RoleDto>> GetPaginationAsync(PaginationParamsWithFilter param, int location)
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
           .Select(r => new
           {
               r.id,
               r.driver_id,
               r.name,
               features = r.feature_roles.Select(fr => new
               {
                   fr.feature,
                   fr.is_allow,
                   fr.is_create,
                   fr.is_modify,
                   fr.is_delete,
                   fr.is_action

               }),
               r.location_id,
               r.is_active
           })
            .ToListAsync();

        var res = data.Select(d => new RoleDto(
            d.id,
            d.driver_id,
            d.name,
            d.features.Select(fr => new FeatureDto(fr.feature.id, fr.feature.name, fr.feature.path, fr.feature.sub_feature.Select(sf => new SubFeatureDto(sf.name, sf.path)).ToList(), fr.is_allow, fr.is_create, fr.is_modify, fr.is_delete, fr.is_action)).ToList(),
            d.location_id,
            d.is_active))
            .ToList();


        return new Pagination<RoleDto>
        {
            Data = res,
            Page = new PaginationData
            {
                TotalCount = count,
                PageNumber = param.PageNumber,
                PageSize = param.PageSize,
                TotalPage = (int)Math.Ceiling(count / (double)param.PageSize)
            }
        };
    }

    public async Task<bool> IsAnyById(int id)
    {
        return await context.role.AnyAsync(x => x.id == id);
    }

    public async Task<bool> IsAnyByNameAsync(string name)
    {
        return await context.role.AnyAsync(x => x.name.Equals(name));
    }

    public async Task<bool> IsAnyReferenceByIdAsync(int id)
    {
        return await context.role.AnyAsync(x => x.id == id && x.operators.Any());
    }
}
