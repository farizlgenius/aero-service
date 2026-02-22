using Aero.Application.DTOs;
using Aero.Application.Interface;
using Aero.Application.Mapper;
using Aero.Domain.Entities;
using Aero.Domain.Interface;
using Aero.Infrastructure.Persistences;
using Microsoft.EntityFrameworkCore;
using System;

namespace Aero.Infrastructure.Repositories;

public class AlvlRepository(AppDbContext context) : IAlvlRepository
{
      public async Task<int> AddAsync(AccessLevel data)
      {

            throw new NotImplementedException();
      }

      public async Task<int> AddCreateAsync(AccessLevel domain)
      {
            await context.access_level.AddAsync(Aero.Infrastructure.Mapper.AccessLevelMapper.ToDb(domain));
            return await context.SaveChangesAsync();
      }

      public async Task<int> DeleteByIdAsync(short id)
      {
            var en = await context.access_level
            .Where(x => x.driver_id == id)
            .OrderBy(x => x.driver_id)
            .FirstOrDefaultAsync();

            if(en is null) return 0;

            context.access_level.Remove(en);
            return await context.SaveChangesAsync();
      }

      public async Task<int> UpdateAsync(AccessLevel newData)
      {
            throw new NotImplementedException();
      }

      public async Task<int> UpdateCreateAsync(AccessLevel domain)
      {
            var en = await context.access_level
            .Where(x => x.driver_id == domain.ComponentId)
            .OrderBy(x => x.driver_id)
            .FirstOrDefaultAsync();

            if(en is null) return 0;

            var e = await context.access_level_component
            .Where(x => x.access_level_id == domain.ComponentId)
            .ToArrayAsync();

            context.access_level_component.RemoveRange(e);

            Aero.Infrastructure.Mapper.AccessLevelMapper.Update(domain,en);

            return await context.SaveChangesAsync();
      }

    public async Task<int> CountByLocationIdAndUpdateTimeAsync(short locationId, DateTime sync)
    {
        var res = await context.access_level
        .AsNoTracking()
        .Where(x => x.location_id == locationId && x.updated_date > sync)
        .CountAsync();

        return res;
    }

    public async Task<string> GetAcrNameByIdAndMacAsync(short component, string mac)
    {
        var res = await context.door.AsNoTracking()
        .OrderBy(x => x.component_id)
        .Where(x => x.component_id == component && x.mac == mac)
        .Select(x => x.name)
        .FirstOrDefaultAsync();

        return res;
    }

    public async Task<IEnumerable<AccessLevel>> GetDomainAsync()
    {
        var res = await context.access_level
        .AsNoTracking()
        .OrderBy(x => x.component_id)
        .Select(x => new AccessLevel
        {
            // Base

            LocationId = x.location_id,
            IsActive = x.is_active,

            // 
            Name = x.name,
            ComponentId = x.component_id,
            Components = x.components.Select(x => new AccessLevelComponent
            {
                Mac = x.mac,
                AcrId = x.acr_id,
                TimezoneId = x.timezone_id,
                DoorId = x.door_id,
                AlvlId = x.alvl_id
            }).ToList()

        }).ToArrayAsync();

        return res;

    }

    public async Task<IEnumerable<AccessLevelDto>> GetAsync()
    {
        var res = await context.access_level
        .AsNoTracking()
        .OrderBy(x => x.component_id)
        .Select(x => new AccessLevelDto
        {
            // Base

            LocationId = x.location_id,
            IsActive = x.is_active,

            // 
            Name = x.name,
            ComponentId = x.component_id,
            Components = x.components.Select(x => new AccessLevelComponentDto
            {
                Mac = x.mac,
                AcrId = x.acr_id,
                TimezoneId = x.timezone_id,
                DoorId = x.door_id,
                AlvlId = x.alvl_id
            }).ToList()

        }).ToArrayAsync();

        return res;

    }

    public async Task<AccessLevelDto> GetByIdAsync(short id)
    {
        var res = await context.access_level
        .AsNoTracking()
        .Where(x => x.driver_id == id)
        .OrderBy(x => x.driver_id)
        .Select(x => new AccessLevelDto
        {
            // Base

            LocationId = x.location_id,
            IsActive = x.is_active,

            // 
            Name = x.name,
            ComponentId = x.component_id,
            Components = x.components.Select(x => new AccessLevelComponentDto
            {
                Mac = x.mac,
                AcrId = x.acr_id,
                TimezoneId = x.timezone_id,
                DoorId = x.door_id,
                AlvlId = x.alvl_id
            }).ToList()

        }).FirstOrDefaultAsync();

        return res;
    }

    public async Task<IEnumerable<AccessLevelDto>> GetByLocationIdAsync(short locationId)
    {
        var res = await context.access_level
        .AsNoTracking()
        .Where(x => x.location_id == locationId || x.location_id == 1)
        .OrderBy(x => x.component_id)
        .Select(x => new AccessLevelDto
        {
            // Base

            LocationId = x.location_id,
            IsActive = x.is_active,

            // 
            Name = x.name,
            ComponentId = x.component_id,
            Components = x.components.Select(x => new AccessLevelComponentDto
            {
                Mac = x.mac,
                AcrId = x.acr_id,
                TimezoneId = x.timezone_id,
                DoorId = x.door_id,
                AlvlId = x.alvl_id
            }).ToList()

        }).ToArrayAsync();

        return res;
    }

    public async Task<IEnumerable<AccessLevelComponent>> GetDoorComponentFromMacAsync(string mac)
    {
        var res = await context.access_level
        .AsNoTracking()
        .OrderBy(x => x.component_id)
        .Where(x => x.components.Any(x => x.mac.Equals(mac)))
        .SelectMany(x => x.components.Select(x => new AccessLevelComponent
        {
            Mac = x.mac,
            AcrId = x.acr_id,
            TimezoneId = x.timezone_id,
            DoorId = x.door_id,
            AlvlId = x.alvl_id
        })).ToArrayAsync();

        return res;
    }

    public async Task<short> GetLowestUnassignedNumberAsync(int max, string mac)
    {
        if (string.IsNullOrEmpty(mac))
        {
            if (max <= 0) return -1;

            var query = context.access_level
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
        else
        {
            if (max <= 0) return -1;

            var query = context.access_level
                .AsNoTracking()
                .Where(x => x.components.Any(x => x.mac == mac))
                .SelectMany(x => x.components.Select(x => x.alvl_id));

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
    }

    public async Task<string> GetTimezoneNameByIdAsync(short component)
    {
        return await context.timezone.AsNoTracking().Where(x => x.component_id == component).Select(x => x.name).FirstOrDefaultAsync() ?? "";
    }

    public async Task<Pagination<AccessLevelDto>> GetPaginationAsync(PaginationParamsWithFilter param, short location)
    {

        var query = context.access_level.AsNoTracking().AsQueryable();


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

        query = query.Where(x => x.location_id == location);

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
            .Select(x => new AccessLevelDto
            {
                // Base

                LocationId = x.location_id,
                IsActive = x.is_active,

                // 
                Name = x.name,
                ComponentId = x.component_id,
                Components = x.components.Select(x => new AccessLevelComponentDto
                {
                    Mac = x.mac,
                    AcrId = x.acr_id,
                    TimezoneId = x.timezone_id,
                    DoorId = x.door_id,
                    AlvlId = x.alvl_id
                }).ToList()

            })
            .ToListAsync();


        return new Pagination<AccessLevelDto>
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

    // public async Task<List<string>> GetUniqueMacFromDoorIdAsync(short doorId)
    // {
    //       var res = await context.access_level
    //       .AsNoTracking()
    //       .Where(x => x.accesslevel_door_timezones.Any(x => x.door_id == doorId))
    //       .SelectMany(x => x.accesslevel_door_timezones.Select(x => x.door.mac))
    //       .Distinct()
    //       .ToListAsync();

    //       return res;
    // }

    public async Task<bool> IsAnyById(short id)
    {
        return await context.access_level.AsNoTracking().AnyAsync(x => x.driver_id == id);
    }
}
