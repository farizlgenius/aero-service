using System;
using Aero.Application.DTOs;
using Aero.Application.Interfaces;
using Aero.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Aero.Infrastructure.Repositories;

public sealed class QIntervalRepository(AppDbContext context) : IQIntervalRepository
{
      public async Task<IEnumerable<IntervalDto>> GetAsync()
      {
            var res = await context.interval
            .AsNoTracking()
            .Select(x => new IntervalDto
            {
                // Base
                Uuid = x.uuid,
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
                Uuid = x.uuid,
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
            .Where(x => x.location_id == locationId)
            .OrderBy(x => x.component_id)
            .Select(x => new IntervalDto
            {
                // Base
                Uuid = x.uuid,
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
                Uuid = x.uuid,
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

      public async Task<short> GetLowestUnassignedNumberAsync(int max)
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
}
