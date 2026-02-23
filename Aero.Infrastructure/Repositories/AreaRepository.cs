using Aero.Application.DTOs;
using Aero.Application.Interface;
using Aero.Application.Mapper;
using Aero.Domain.Entities;
using Aero.Domain.Interface;
using Aero.Infrastructure.Persistences;
using Mapster;
using Microsoft.EntityFrameworkCore;
using System;

namespace Aero.Infrastructure.Repositories;

public class AreaRepository(AppDbContext context) : IAreaRepository
{
      public async Task<int> AddAsync(AccessArea data)
      {
            var en = new Aero.Infrastructure.Persistences.Entities.AccessArea(data.DriverId,data.Name,data.MultiOccupancy,data.AccessControl,data.OccControl,data.OccSet,data.OccMax,data.OccUp,data.OccDown,data.AreaFlag,data.LocationId);
            await context.area.AddAsync(en);
            return await context.SaveChangesAsync();
      }

      public async Task<int> DeleteByIdAsync(int id)
      {
             var en = await context.area
            .Where(x => x.id == id)
            .OrderBy(x => x.id)
            .FirstOrDefaultAsync();

             if(en is null) return 0;

            context.area.Remove(en);
            return await context.SaveChangesAsync();

      }

      public async Task<int> UpdateAsync(AccessArea data)
      {
            var en = await context.area
            .Where(x => x.id == data.Id)
            .OrderBy(x => x.id)
            .FirstOrDefaultAsync();

            if(en is null) return 0;

            en.Update(data);

            context.area.Update(en);
            return await context.SaveChangesAsync();
      }

    public async Task<int> CountByLocationIdAndUpdateTimeAsync(int locationId, DateTime sync)
    {
        var res = await context.area
        .AsNoTracking()
        .Where(x => x.location_id == locationId && x.updated_date > sync)
        .CountAsync();

        return res;
    }

    public async Task<IEnumerable<Mode>> GetAccessControlOptionAsync()
    {
        var dto = await context.area_access_control
            .Select(x => new Mode
            {
                Name = x.name,
                Value = x.value,
                Description = x.description
            })
            .ToArrayAsync();

        return dto;
    }

    public async Task<IEnumerable<Mode>> GetAreaFlagOptionAsync()
    {
        var dto = await context.area_flag
          .Select(x => new Mode
          {
              Name = x.name,
              Value = x.value,
              Description = x.description
          })
          .ToArrayAsync();

        return dto;
    }

    public async Task<IEnumerable<AccessAreaDto>> GetAsync()
    {
        var res = await context.area
        .AsNoTracking()
        .Select(a => new AccessAreaDto(a.id,a.name,a.multi_occ,a.access_control,a.occ_control,a.occ_set,a.occ_max,a.occ_up,a.occ_down,a.area_flag,a.location_id,a.is_active))
        .ToArrayAsync();

        return res;
    }

    public async Task<AccessAreaDto> GetByIdAsync(int id)
    {
        var res = await context.area
        .AsNoTracking()
        .Where(x => x.id == id)
        .OrderBy(x => x.id)
        .Select(a => new AccessAreaDto(a.id, a.name, a.multi_occ, a.access_control, a.occ_control, a.occ_set, a.occ_max, a.occ_up, a.occ_down, a.area_flag,a.location_id,a.is_active))
        .FirstOrDefaultAsync();

        return res;
    }

    public async Task<IEnumerable<AccessAreaDto>> GetByLocationIdAsync(int locationId)
    {
        var res = await context.area
        .AsNoTracking()
        .Where(x => x.location_id == locationId || x.location_id == 1)
        .OrderBy(x => x.id)
        .Select(a => new AccessAreaDto(a.id, a.name, a.multi_occ, a.access_control, a.occ_control, a.occ_set, a.occ_max, a.occ_up, a.occ_down, a.area_flag, a.location_id, a.is_active))
        .ToArrayAsync();

        return res;
    }

    public async Task<IEnumerable<Mode>> GetCommandAsync()
    {
        var dto = await context.access_area_command
            .Select(x => new Mode
            {
                Name = x.name,
                Value = x.value,
                Description = x.description
            })
            .ToArrayAsync();

        return dto;
    }

    public async Task<short> GetLowestUnassignedNumberAsync(int max, string mac)
    {
        if (max <= 0) return -1;

        var query = context.area
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

    public async Task<IEnumerable<Mode>> GetMultiOccupancyOptionAsync()
    {
        var dto = await context.multi_occupancy
           .Select(x => new Mode
           {
               Name = x.name,
               Value = x.value,
               Description = x.description
           })
           .ToArrayAsync();

        return dto;
    }

    public async Task<IEnumerable<Mode>> GetOccupancyControlOptionAsync()
    {
        var dto = await context.occupancy_control
           .Select(x => new Mode
           {
               Name = x.name,
               Value = x.value,
               Description = x.description
           })
           .ToArrayAsync();

        return dto;

    }

    public async Task<Pagination<AccessAreaDto>> GetPaginationAsync(PaginationParamsWithFilter param, int location)
    {

        var query = context.area.AsNoTracking().AsQueryable();


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
            .Select(a => new AccessAreaDto(a.id, a.name, a.multi_occ, a.access_control, a.occ_control, a.occ_set, a.occ_max, a.occ_up, a.occ_down, a.area_flag, a.location_id, a.is_active))
            .ToListAsync();


        return new Pagination<AccessAreaDto>
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

    public async Task<bool> IsAnyById(int id)
    {
        return await context.area.AnyAsync(x => x.id == id);
    }

    public async Task<bool> IsAnyByNameAsync(string name)
    {
        return await context.area.AnyAsync(x => x.name.Equals(name.Trim()));
    }
}
