using Aero.Application.DTOs;
using Aero.Domain.Entities;
using Aero.Domain.Interface;
using Aero.Infrastructure.Persistences;
using Aero.Infrastructure.Mapper;
using Microsoft.EntityFrameworkCore;
using System;
using Aero.Application.Interface;

namespace Aero.Infrastructure.Repositories;

public class TzRepository(AppDbContext context) : ITzRepository
{
      public async Task<int> AddAsync(Timezone data,short ComponentId)
      {     
            data.ComponentId = ComponentId;
            await context.timezone.AddAsync(TimezoneMapper.ToEf(data));
            return await context.SaveChangesAsync();
      }

      public async Task<int> AddAsync(Timezone data)
      {
        await context.timezone.AddAsync(TimezoneMapper.ToEf(data));
        return await context.SaveChangesAsync();
    }

      public async Task<int> DeleteByComponentIdAsync(short component)
      {
            var en = await context.timezone
            .Where(x => x.component_id == component)
            .OrderBy(x => x.component_id)
            .FirstOrDefaultAsync();

            if(en is null) return 0;

            context.timezone.Remove(en);
            return await context.SaveChangesAsync();
      }

      public async Task<int> UpdateAsync(Timezone d)
      {
            var en = await context.timezone
            .Where(x => x.component_id == d.ComponentId)
            .OrderBy(x => x.component_id)
            .FirstOrDefaultAsync();

            if(en is null) return 0;

            // Update Process
            en.name = d.Name;
            en.mode = d.Mode;
            en.active_time = d.ActiveTime;
            en.deactive_time = d.DeactiveTime;

            context.timezone.Update(en);

            return await context.SaveChangesAsync();
      }

    public async Task<int> CountByLocationIdAndUpdateTimeAsync(short locationId, DateTime sync)
    {
        var res = await context.timezone
        .AsNoTracking()
        .Where(x => x.location_id == locationId && x.updated_date > sync)
        .CountAsync();

        return res;
    }

    public async Task<IEnumerable<TimeZoneDto>> GetAsync()
    {
        var res = await context.timezone
        .AsNoTracking()
        .Select(x => new TimeZoneDto
        {
            // Base
            IsActive = x.is_active,

            // extend_desc
            ComponentId = x.component_id,
            Name = x.name,
            Mode = x.mode,
            ActiveTime = x.active_time,
            DeactiveTime = x.deactive_time,
            Intervals = x.timezone_intervals
                    .Select(s => s.interval)
                    .Select(p => new IntervalDto
                    {
                        // Base 
                        IsActive = p.is_active,
                        LocationId = p.location_id,

                        // extend_desc
                        ComponentId = p.component_id,
                        DaysDesc = p.days_detail,
                        StartTime = p.start_time,
                        EndTime = p.end_time,
                        Days = new DaysInWeekDto
                        {
                            Sunday = p.days.sunday,
                            Monday = p.days.monday,
                            Tuesday = p.days.tuesday,
                            Wednesday = p.days.wednesday,
                            Thursday = p.days.thursday,
                            Friday = p.days.friday,
                            Saturday = p.days.saturday
                        }

                    })
                    .ToList(),

        })
        .ToArrayAsync();

        return res;
    }

    public async Task<TimeZoneDto> GetByComponentIdAsync(short componentId)
    {
        var res = await context.timezone
    .AsNoTracking()
    .Where(x => x.component_id == componentId)
    .Select(x => new TimeZoneDto
    {
        // Base
        IsActive = x.is_active,

        // extend_desc
        ComponentId = x.component_id,
        Name = x.name,
        Mode = x.mode,
        ActiveTime = x.active_time,
        DeactiveTime = x.deactive_time,
        Intervals = x.timezone_intervals
                .Select(s => s.interval)
                .Select(p => new IntervalDto
                {
                    // Base 
                    IsActive = p.is_active,
                    LocationId = p.location_id,

                    // extend_desc
                    ComponentId = p.component_id,
                    DaysDesc = p.days_desc,
                    StartTime = p.start_time,
                    EndTime = p.end_time,
                    Days = new DaysInWeekDto
                    {
                        Sunday = p.days.sunday,
                        Monday = p.days.monday,
                        Tuesday = p.days.tuesday,
                        Wednesday = p.days.wednesday,
                        Thursday = p.days.thursday,
                        Friday = p.days.friday,
                        Saturday = p.days.saturday
                    }

                })
                .ToList(),

    })
    .FirstOrDefaultAsync();

        return res;
    }

    public async Task<IEnumerable<TimeZoneDto>> GetByLocationIdAsync(short locationId)
    {
        var res = await context.timezone
    .AsNoTracking()
    .Where(x => x.location_id == locationId || x.location_id == 1)
    .Select(x => new TimeZoneDto
    {
        // Base
        IsActive = x.is_active,

        // extend_desc
        ComponentId = x.component_id,
        Name = x.name,
        Mode = x.mode,
        ActiveTime = x.active_time,
        DeactiveTime = x.deactive_time,
        Intervals = x.timezone_intervals
                .Select(s => s.interval)
                .Select(p => new IntervalDto
                {
                    // Base 
                    IsActive = p.is_active,
                    LocationId = p.location_id,

                    // extend_desc
                    ComponentId = p.component_id,
                    DaysDesc = p.days_detail,
                    StartTime = p.start_time,
                    EndTime = p.end_time,
                    Days = new DaysInWeekDto
                    {
                        Sunday = p.days.sunday,
                        Monday = p.days.monday,
                        Tuesday = p.days.tuesday,
                        Wednesday = p.days.wednesday,
                        Thursday = p.days.thursday,
                        Friday = p.days.friday,
                        Saturday = p.days.saturday
                    }

                })
                .ToList(),

    })
    .ToArrayAsync();

        return res;
    }

    public async Task<short> GetLowestUnassignedNumberAsync(int max, string mac)
    {
        if (max <= 0) return -1;

        var query = context.timezone
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

    public async Task<IEnumerable<Mode>> GetCommandAsync()
    {
        var dtos = await context.timezone_command.AsNoTracking().Select(s => new Mode
        {
            Name = s.name,
            Value = s.value,
            Description = s.description,

        }).ToArrayAsync();

        return dtos;
    }

    public async Task<bool> IsAnyByComponentId(short component)
    {
        return await context.timezone.AnyAsync(x => x.component_id == component);
    }

    public async Task<IEnumerable<Mode>> GetModeAsync()
    {
        var dtos = await context.timezone_mode.AsNoTracking().Select(s => new Mode
        {
            Name = s.name,
            Value = s.value,
            Description = s.description,

        }).ToArrayAsync();

        return dtos;
    }

    public async Task<Pagination<TimeZoneDto>> GetPaginationAsync(PaginationParamsWithFilter param, short location)
    {

        var query = context.timezone.AsNoTracking().AsQueryable();


        if (!string.IsNullOrWhiteSpace(param.Search))
        {
            if (!string.IsNullOrWhiteSpace(param.Search))
            {
                var search = param.Search.Trim();

                if (context.Database.IsNpgsql())
                {
                    var pattern = $"%{search}%";

                    query = query.Where(x =>
                        EF.Functions.ILike(x.name, pattern) ||
                        EF.Functions.ILike(x.active_time, pattern) ||
                        EF.Functions.ILike(x.deactive_time, pattern)

                    );
                }
                else // SQL Server
                {
                    query = query.Where(x =>
                        x.name.Contains(search) ||
                        x.active_time.Contains(search) ||
                        x.deactive_time.Contains(search)
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
           .Select(x => new TimeZoneDto
           {
               // Base
               IsActive = x.is_active,

               // extend_desc
               ComponentId = x.component_id,
               Name = x.name,
               Mode = x.mode,
               ActiveTime = x.active_time,
               DeactiveTime = x.deactive_time,
               Intervals = x.timezone_intervals
                .Select(s => s.interval)
                .Select(p => new IntervalDto
                {
                    // Base 
                    IsActive = p.is_active,
                    LocationId = p.location_id,

                    // extend_desc
                    ComponentId = p.component_id,
                    DaysDesc = p.days_detail,
                    StartTime = p.start_time,
                    EndTime = p.end_time,
                    Days = new DaysInWeekDto
                    {
                        Sunday = p.days.sunday,
                        Monday = p.days.monday,
                        Tuesday = p.days.tuesday,
                        Wednesday = p.days.wednesday,
                        Thursday = p.days.thursday,
                        Friday = p.days.friday,
                        Saturday = p.days.saturday
                    }

                })
                .ToList(),

           })
            .ToListAsync();


        return new Pagination<TimeZoneDto>
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
