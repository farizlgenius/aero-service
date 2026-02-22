using Aero.Application.DTOs;
using Aero.Domain.Entities;
using Aero.Domain.Interface;
using Aero.Infrastructure.Persistences;
using Aero.Infrastructure.Mapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Cryptography.X509Certificates;
using Aero.Application.Interface;

namespace Aero.Infrastructure.Repositories;

public class IntervalRepository(AppDbContext context) : IIntervalRepository
{
      public async Task<int> AddAsync(Interval data)
      {
           await context.interval.AddAsync(IntervalMapper.ToEf(data)); 
           return await context.SaveChangesAsync();
      }

      public async Task<int> DeleteByComponentIdAsync(short component)
      {
            var en = await context.interval
            .Include(x => x.timezone_intervals)
            .Where(x => x.component_id == component)
            .OrderBy(x => x.component_id)
            .FirstOrDefaultAsync();

            if(en is null) return 0;

            context.timezone_interval.RemoveRange(en.timezone_intervals);
            context.interval.Remove(en);
            return await context.SaveChangesAsync();
      }

      public async Task<int> UpdateAsync(Interval newData)
      {
            var en = await context.interval
            .Include(x => x.days)
            .Where(x => x.component_id == newData.ComponentId)
            .OrderBy(x => x.component_id)
            .FirstOrDefaultAsync();

            if(en is null) return 0;

            IntervalMapper.Update(en,newData);

            context.interval.Update(en);
            return await context.SaveChangesAsync();
            
      }
    public async Task<IEnumerable<IntervalDto>> GetAsync()
    {
        var res = await context.interval
        .AsNoTracking()
        .Select(x => new IntervalDto
        {
            // Base
            IsActive = x.is_active,
            LocationId = x.location_id,

            // extend_desc
            ComponentId = x.component_id,
            DaysDesc = x.days_desc,
            StartTime = x.start_time,
            EndTime = x.end_time,
            Days = new DaysInWeekDto
            {
                Sunday = x.days.sunday,
                Monday = x.days.monday,
                Tuesday = x.days.tuesday,
                Wednesday = x.days.wednesday,
                Thursday = x.days.thursday,
                Friday = x.days.friday,
                Saturday = x.days.saturday,
            }
        })
        .ToArrayAsync();

        return res;
    }

    public async Task<IntervalDto> GetByComponentIdAsync(short componentId)
    {
        var res = await context.interval
        .AsNoTracking()
        .Where(x => x.component_id == componentId)
        .OrderBy(x => x.component_id)
        .Select(x => new IntervalDto
        {
            // Base
            IsActive = x.is_active,
            LocationId = x.location_id,

            // extend_desc
            ComponentId = x.component_id,
            DaysDesc = x.days_desc,
            StartTime = x.start_time,
            EndTime = x.end_time,
            Days = new DaysInWeekDto
            {
                Sunday = x.days.sunday,
                Monday = x.days.monday,
                Tuesday = x.days.tuesday,
                Wednesday = x.days.wednesday,
                Thursday = x.days.thursday,
                Friday = x.days.friday,
                Saturday = x.days.saturday,
            }
        })
        .FirstOrDefaultAsync();

        return res;
    }

    public async Task<IEnumerable<IntervalDto>> GetByLocationIdAsync(short locationId)
    {
        var res = await context.interval
        .AsNoTracking()
        .Where(x => x.location_id == locationId || x.location_id == 1)
        .OrderBy(x => x.component_id)
        .Select(x => new IntervalDto
        {
            // Base
            IsActive = x.is_active,
            LocationId = x.location_id,

            // extend_desc
            ComponentId = x.component_id,
            DaysDesc = x.days_desc,
            StartTime = x.start_time,
            EndTime = x.end_time,
            Days = new DaysInWeekDto
            {
                Sunday = x.days.sunday,
                Monday = x.days.monday,
                Tuesday = x.days.tuesday,
                Wednesday = x.days.wednesday,
                Thursday = x.days.thursday,
                Friday = x.days.friday,
                Saturday = x.days.saturday,
            }
        })
        .ToArrayAsync();

        return res;
    }

    public async Task<IEnumerable<IntervalDto>> GetIntervalFromTimezoneComponentIdAsync(short component)
    {
        return await context.interval.AsNoTracking()
        .Where(x => x.timezone_intervals.Any(x => x.timezone_id == component))
        .OrderBy(x => x.component_id)
        .Select(x => new IntervalDto
        {
            // Base
            IsActive = x.is_active,
            LocationId = x.location_id,

            // extend_desc
            ComponentId = x.component_id,
            DaysDesc = x.days_desc,
            StartTime = x.start_time,
            EndTime = x.end_time,
            Days = new DaysInWeekDto
            {
                Sunday = x.days.sunday,
                Monday = x.days.monday,
                Tuesday = x.days.tuesday,
                Wednesday = x.days.wednesday,
                Thursday = x.days.thursday,
                Friday = x.days.friday,
                Saturday = x.days.saturday,
            }
        })
        .ToArrayAsync();
    }

    public async Task<short> GetLowestUnassignedNumberAsync(int max, string mac)
    {
        if (max <= 0) return -1;

        var query = context.interval
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

    public async Task<IEnumerable<short>> GetTimezoneIntervalIdByIntervalComponentIdAsync(short component)
    {
        return await context.interval
        .AsNoTracking()
        .Where(x => x.component_id == component)
        .SelectMany(x => x.timezone_intervals.Select(x => x.timezone_id).ToArray())
        .ToArrayAsync();

    }

    public async Task<bool> IsAnyByComponentId(short component)
    {
        return await context.interval.AsNoTracking().AnyAsync(x => x.component_id == component);
    }

    public async Task<bool> IsAnyOnEachDays(IntervalDto dto)
    {
        return await context.interval.AnyAsync(p =>
        p.start_time == dto.StartTime &&
        p.end_time == dto.EndTime &&
        p.days.sunday == dto.Days.Sunday &&
        p.days.monday == dto.Days.Monday &&
        p.days.tuesday == dto.Days.Tuesday &&
        p.days.wednesday == dto.Days.Wednesday &&
        p.days.thursday == dto.Days.Thursday &&
        p.days.friday == dto.Days.Friday &&
        p.days.saturday == dto.Days.Saturday
        );
    }

    public async Task<bool> IsAnyReferenceByComponentAsync(short component)
    {
        return await context.interval
           .AsNoTracking()
           .AnyAsync(x => x.component_id == component && x.timezone_intervals.Any());
    }

    public async Task<int> CountByLocationIdAndUpdateTimeAsync(short locationId, DateTime sync)
    {
        var res = await context.interval
        .AsNoTracking()
        .Where(x => x.location_id == locationId && x.updated_date < sync)
        .CountAsync();

        return res;
    }

    public async Task<Pagination<IntervalDto>> GetPaginationAsync(PaginationParamsWithFilter param, short location)
    {

        var query = context.interval.AsNoTracking().AsQueryable();


        if (!string.IsNullOrWhiteSpace(param.Search))
        {
            if (!string.IsNullOrWhiteSpace(param.Search))
            {
                var search = param.Search.Trim();

                if (context.Database.IsNpgsql())
                {
                    var pattern = $"%{search}%";

                    query = query.Where(x =>
                        EF.Functions.ILike(x.days_desc, pattern) ||
                        EF.Functions.ILike(x.start_time, pattern) ||
                        EF.Functions.ILike(x.end_time, pattern)

                    );
                }
                else // SQL Server
                {
                    query = query.Where(x =>
                        x.days_desc.Contains(search) ||
                        x.start_time.Contains(search) ||
                        x.end_time.Contains(search)
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
            .Select(x => new IntervalDto
            {
                // Base
                IsActive = x.is_active,
                LocationId = x.location_id,

                // extend_desc
                ComponentId = x.component_id,
                DaysDesc = x.days_desc,
                StartTime = x.start_time,
                EndTime = x.end_time,
                Days = new DaysInWeekDto
                {
                    Sunday = x.days.sunday,
                    Monday = x.days.monday,
                    Tuesday = x.days.tuesday,
                    Wednesday = x.days.wednesday,
                    Thursday = x.days.thursday,
                    Friday = x.days.friday,
                    Saturday = x.days.saturday,
                }
            })
            .ToListAsync();


        return new Pagination<IntervalDto>
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
