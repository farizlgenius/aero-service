using Aero.Application.DTOs;
using Aero.Domain.Entities;
using Aero.Domain.Interface;
using Aero.Infrastructure.Persistences;
using Aero.Infrastructure.Mapper;
using Microsoft.EntityFrameworkCore;
using System;
using Aero.Application.Interface;

namespace Aero.Infrastructure.Repositories;

public class LocationRepository(AppDbContext context) : ILocationRepository
{
      public async Task<int> AddAsync(Aero.Domain.Entities.Location data)
      {
            var en = LocationMapper.ToEf(data);
            await context.location.AddAsync(en);
            return await context.SaveChangesAsync();
      }

      public async Task<int> DeleteByComponentIdAsync(short component)
      {
            var en = await context.location
            .OrderBy(x => x.component_id)
            .Where(x => x.component_id == component)
            .FirstOrDefaultAsync();

            if(en is null) return 0;

            context.location.Remove(en);
            return await context.SaveChangesAsync();
      }

      public async Task<int> UpdateAsync(Location newData)
      {
            var en = await context.location
            .OrderBy(x => x.component_id)
            .Where(x => x.component_id == newData.ComponentId)
            .FirstOrDefaultAsync();

            if(en is null) return 0;

            LocationMapper.Update(newData,en);

            context.location.Update(en);
            return await context.SaveChangesAsync();
      }

    public async Task<List<string>> CheckRelateReferenceByComponentIdAsync(short component)
    {
        List<string> errors = new List<string>();
        // hardware
        if (await context.location
            .AnyAsync(x => x.component_id == component && x.hardwares.Count() > 0)
            )
        {
            errors.Add("Found relate hardware");
        }

        // modules
        if (await context.location
            .AnyAsync(x => x.component_id == component && x.modules.Count() > 0)
            )
        {
            errors.Add("Found relate modules");
        }

        // CP
        if (await context.location
            .AnyAsync(x => x.component_id == component && x.control_points.Count() > 0)
            )
        {
            errors.Add("Found relate control point");
        }

        // MP
        if (await context.location
            .AnyAsync(x => x.component_id == component && x.monitor_points.Count() > 0)
            )
        {
            errors.Add("Found relate control point");
        }

        // ALVL
        if (await context.location
            .AnyAsync(x => x.component_id == component && x.accesslevels.Count() > 0)
            )
        {
            errors.Add("Found relate access level");
        }

        // AREA
        if (await context.location
            .AnyAsync(x => x.component_id == component && x.areas.Count() > 0)
            )
        {
            errors.Add("Found relate access level");
        }

        // Card Holders
        if (await context.location
            .AnyAsync(x => x.component_id == component && x.cardholders.Count() > 0)
            )
        {
            errors.Add("Found relate card holder");
        }

        // door 
        if (await context.location
            .AnyAsync(x => x.component_id == component && x.doors.Count() > 0)
            )
        {
            errors.Add("Found relate door");
        }

        // MPG 
        if (await context.location
            .AnyAsync(x => x.component_id == component && x.monitor_groups.Count() > 0)
            )
        {
            errors.Add("Found relate monitor group");
        }

        // operator 
        if (await context.location
            .AnyAsync(x => x.component_id == component && x.operator_locations.Count() > 1)
            )
        {
            errors.Add("Found relate operator");
        }

        // Holiday 
        if (await context.location
            .AnyAsync(x => x.component_id == component && x.holidays.Count() > 0)
            )
        {
            errors.Add("Found relate holiday");
        }

        // Cred 
        if (await context.location
            .AnyAsync(x => x.component_id == component && x.credentials.Count() > 0)
            )
        {
            errors.Add("Found relate credential");
        }

        // reader 
        if (await context.location
            .AnyAsync(x => x.component_id == component && x.readers.Count() > 0)
            )
        {
            errors.Add("Found relate reader");
        }

        // rex 
        if (await context.location
            .AnyAsync(x => x.component_id == component && x.request_exits.Count() > 0)
            )
        {
            errors.Add("Found relate rex");
        }

        // strk 
        if (await context.location
            .AnyAsync(x => x.component_id == component && x.strikes.Count() > 0)
            )
        {
            errors.Add("Found relate strike");
        }

        // proc 
        if (await context.location
            .AnyAsync(x => x.component_id == component && x.procedures.Count() > 0)
            )
        {
            errors.Add("Found relate procedure");
        }

        // ac 
        if (await context.location
            .AnyAsync(x => x.component_id == component && x.actions.Count() > 0)
            )
        {
            errors.Add("Found relate procedure");
        }

        // trigger 
        if (await context.location
            .AnyAsync(x => x.component_id == component && x.triggers.Count() > 0)
            )
        {
            errors.Add("Found relate trigger");
        }

        // interval 
        if (await context.location
            .AnyAsync(x => x.component_id == component && x.intervals.Count() > 0)
            )
        {
            errors.Add("Found relate interval");
        }

        // timezone 
        if (await context.location
            .AnyAsync(x => x.component_id == component && x.timezones.Count() > 0)
            )
        {
            errors.Add("Found relate timezone");
        }

        return errors;
    }

    public async Task<IEnumerable<LocationDto>> GetAsync()
    {
        var res = await context.location
        .AsNoTracking()
        .Select(x => new LocationDto
        {

            ComponentId = x.component_id,
            LocationName = x.location_name,
            Description = x.description,
        })
        .ToArrayAsync();

        return res;
    }

    public async Task<LocationDto> GetByComponentIdAsync(short componentId)
    {
        var res = await context.location
       .AsNoTracking()
       .OrderBy(x => x.component_id)
       .Where(x => x.component_id == componentId)
       .Select(x => new LocationDto
       {
           ComponentId = x.component_id,
           LocationName = x.location_name,
           Description = x.description,
       })
       .FirstOrDefaultAsync();

        return res;
    }

    public async Task<IEnumerable<LocationDto>> GetByLocationIdAsync(short locationId)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<LocationDto>> GetLocationsByListIdAsync(LocationRangeDto dto)
    {
        var dtos = await context.location
            .AsNoTracking()
            .OrderBy(x => x.component_id)
            .Where(x => dto.locationIds.Contains(x.component_id))
            .Select(x => new LocationDto
            {
                ComponentId = x.component_id,
                LocationName = x.location_name,
                Description = x.description,
            })
            .ToArrayAsync();

        return dtos;
    }

    public async Task<short> GetLowestUnassignedNumberAsync(int max, string mac)
    {
        if (max <= 0) return -1;

        var query = context.location
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
        return await context.location.AnyAsync(x => x.component_id == component);
    }

    public async Task<bool> IsAnyByLocationNameAsync(string name)
    {
        return await context.location.AnyAsync(x => x.location_name.Equals(name));
    }

    public async Task<Pagination<LocationDto>> GetPaginationAsync(PaginationParamsWithFilter param, short location)
    {
        var query = context.location.AsNoTracking().AsQueryable();


        if (!string.IsNullOrWhiteSpace(param.Search))
        {
            if (!string.IsNullOrWhiteSpace(param.Search))
            {
                var search = param.Search.Trim();

                if (context.Database.IsNpgsql())
                {
                    var pattern = $"%{search}%";

                    query = query.Where(x =>
                        EF.Functions.ILike(x.location_name, pattern) ||
                        EF.Functions.ILike(x.description, pattern)
                    );
                }
                else // SQL Server
                {
                    query = query.Where(x =>
                        x.location_name.Contains(search) ||
                        x.description.Contains(search)
                    );
                }
            }
        }


        query = query.Where(x => x.component_id != 1);

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
            .Select(x => new LocationDto
            {
                ComponentId = x.component_id,
                LocationName = x.location_name,
                Description = x.description

            })
            .ToListAsync();


        return new Pagination<LocationDto>
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
}
