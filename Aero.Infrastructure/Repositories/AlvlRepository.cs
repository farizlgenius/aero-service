using Aero.Application.DTOs;
using Aero.Application.Interface;
using Aero.Domain.Entities;
using Aero.Domain.Interface;
using Aero.Infrastructure.Persistences;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Aero.Infrastructure.Repositories;

public class AlvlRepository(AppDbContext context) : IAlvlRepository
{
      public async Task<int> AddAsync(AccessLevel data)
      {
            var en = new Aero.Infrastructure.Persistences.Entities.AccessLevel(
                data.Name,
                data.Components.Select(x => new Aero.Infrastructure.Persistences.Entities.AccessLevelComponent(x.DriverId,x.DeviceId,x.DoorId,x.AcrId,x.TimezoneId)).ToList(),
                data.LocationId
               );
            await context.access_level.AddAsync(en);
            var status = await context.SaveChangesAsync();
        if (status <= 0) return -1;
        return en.id;
        
      }


      public async Task<int> DeleteByIdAsync(int id)
      {
            var en = await context.access_level
            .Where(x => x.id == id)
            .OrderBy(x => x.id)
            .FirstOrDefaultAsync();

            if(en is null) return 0;

            context.access_level.Remove(en);
            return await context.SaveChangesAsync();
      }

      public async Task<int> UpdateAsync(AccessLevel data)
      {
        var en = await context.access_level
         .Where(x => x.id == data.Id)
         .OrderBy(x => x.id)
         .FirstOrDefaultAsync();

        if (en is null) return 0;

        var driverids = data.Components.Select(x => x.DriverId).Distinct().ToList();

        var e = await context.access_level_component
        .Where(x => driverids.Contains(x.access_level_id))
        .ToArrayAsync();

        context.access_level_component.RemoveRange(e);

        en.Update(data);

        return await context.SaveChangesAsync();
    }


    public async Task<int> CountByLocationIdAndUpdateTimeAsync(int locationId, DateTime sync)
    {
        var res = await context.access_level
        .AsNoTracking()
        .Where(x => x.location_id == locationId && x.updated_date > sync)
        .CountAsync();

        return res;
    }

    public async Task<string> GetAcrNameByIdAndDeviceIdAsync(int id, int deviceId)
    {
        var res = await context.door.AsNoTracking()
        .OrderBy(x => x.id)
        .Where(x => x.id == id && x.device_id == deviceId)
        .Select(x => x.name)
        .FirstOrDefaultAsync();

        return res;
    }

    public async Task<IEnumerable<AccessLevel>> GetDomainAsync()
    {
        var data = await context.access_level
        .AsNoTracking()
        .OrderBy(x => x.id)
        .Select(x => new
        {
            x.id,
            x.name,
            components = x.components.Select(c => new AccessLevelComponent(c.driver_id, c.device_id, c.door_id, c.acr_id, c.timezone_id)),
            x.location_id,
            x.is_active
        }).ToArrayAsync();

        var res = data.Select(x => new AccessLevel(x.id,x.name,x.components.ToList(),x.location_id,x.is_active)).ToList();

        return res;

    }

    public async Task<IEnumerable<AccessLevelDto>> GetAsync()
    {
        var data = await context.access_level
        .AsNoTracking()
        .OrderBy(x => x.id)
        .Select(x => new
        {
            x.id,
            x.name,
            components = x.components.Select(c => new AccessLevelComponentDto(c.driver_id, c.device_id, c.door_id, c.acr_id, c.timezone_id)),
            x.location_id,
            x.is_active
        }).ToArrayAsync();

        var res = data.Select(x => new AccessLevelDto(x.id, x.name, x.components.ToList(), x.location_id,x.is_active)).ToList();

        return res;

    }

    public async Task<AccessLevelDto> GetByIdAsync(int id)
    {
        var data = await context.access_level
        .AsNoTracking()
        .Where(x => x.id == id)
        .OrderBy(x => x.id)
         .Select(x => new
         {
             x.id,
             x.name,
             components = x.components.Select(c => new AccessLevelComponentDto(c.driver_id, c.device_id, c.door_id, c.acr_id, c.timezone_id)),
             x.location_id,
             x.is_active
         }).FirstOrDefaultAsync();

        if (data is null) return null;

        var res = new AccessLevelDto(data.id,  data.name, data.components.ToList(), data.location_id, data.is_active);

        return res;
    }

    public async Task<IEnumerable<AccessLevelDto>> GetByLocationIdAsync(int locationId)
    {
        var data = await context.access_level
        .AsNoTracking()
        .Where(x => x.location_id == locationId || x.location_id == 1)
        .OrderBy(x => x.id)
        .Select(x => new
        {
            x.id,
            x.name,
            components = x.components.Select(c => new AccessLevelComponentDto(c.driver_id, c.device_id, c.door_id, c.acr_id, c.timezone_id)),
            x.location_id,
            x.is_active
        }).ToArrayAsync();

        var res = data.Select(x => new AccessLevelDto(x.id, x.name, x.components.ToList(), x.location_id, x.is_active)).ToList();

        return res;
    }

    public async Task<IEnumerable<AccessLevelComponent>> GetDoorComponentFromMacAsync(string mac)
    {
        var res = await context.access_level
        .AsNoTracking()
        .OrderBy(x => x.id)
        .Where(x => x.components.Any(x => x.device_id.Equals(mac)))
        .SelectMany(x => x.components.Select(x => new AccessLevelComponent(x.driver_id,x.device_id, x.door_id,x.acr_id,x.timezone_id))).ToArrayAsync();

        return res;
    }

    public async Task<short> GetLowestUnassignedNumberAsync(int max, int driverid)
    {
        if (max <= 0) return -1;

        var query = context.access_level
            .AsNoTracking()
            .Where(x => x.components.Any(x => x.driver_id == driverid))
            .SelectMany(x => x.components.Select(x => x.driver_id));

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

    public async Task<string> GetTimezoneNameByIdAsync(int id)
    {
        return await context.timezone.AsNoTracking().Where(x => x.id == id).Select(x => x.name).FirstOrDefaultAsync() ?? "";
    }

    public async Task<Pagination<AccessLevelDto>> GetPaginationAsync(PaginationParamsWithFilter param, int location)
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
            .Select(x => new
            {
                x.id,
                x.name,
                components = x.components.Select(c => new AccessLevelComponentDto(c.driver_id, c.device_id, c.door_id, c.acr_id, c.timezone_id)),
                x.location_id,
                x.is_active
            })
            .ToArrayAsync();

        var res = data.Select(x => new AccessLevelDto(x.id, x.name, x.components.ToList(), x.location_id, x.is_active)).ToList();


        return new Pagination<AccessLevelDto>
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

    public async Task<bool> IsAnyByNameAsync(string name)
    {
        return await context.access_level.AsNoTracking().AnyAsync(x => x.name.Equals(name));
    }

    public async Task<bool> IsAnyById(int id)
    {
        return await context.access_level.AsNoTracking().AnyAsync(x => x.id == id);
    }
}
