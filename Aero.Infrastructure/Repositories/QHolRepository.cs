using System;
using Aero.Application.DTOs;
using Aero.Application.Interfaces;
using Aero.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Aero.Infrastructure.Repositories;

public sealed class QHolRepository(AppDbContext context) : IQHolRepository
{
      public async Task<int> CountByLocationIdAndUpdateTimeAsync(short locationId, DateTime sync)
      {
            var res = await context.holiday
            .AsNoTracking()
            .Where(x => x.location_id == locationId && x.updated_date < sync)
            .CountAsync();

            return res;
      }

      public async Task<IEnumerable<HolidayDto>> GetAsync()
      {
            var res = await context.holiday
            .AsNoTracking()
            .Select(p => new HolidayDto 
            {
                // Base
                Uuid = p.uuid,
                LocationId = p.location_id,
                IsActive = p.is_active,

                // extend_desc
                Day = p.day,
                Month = p.month,
                Year = p.year,
                Extend = p.extend,
                TypeMask = p.type_mask

            }).ToArrayAsync();

            return res;
      }

      public async Task<HolidayDto> GetByComponentIdAsync(short component)
      {
            var res = await context.holiday
            .AsNoTracking()
            .Where(p => p.component_id == component).Select(p => new HolidayDto 
            {
                // Base
                Uuid = p.uuid,
                LocationId = p.location_id,
                IsActive = p.is_active,

                // extend_desc
                Day = p.day,
                Month = p.month,
                Year = p.year,
                Extend = p.extend,
                TypeMask = p.type_mask

            }).FirstOrDefaultAsync();

            return res;
      }

      public async Task<IEnumerable<HolidayDto>> GetByLocationIdAsync(short locationId)
      {
            var res = await context.holiday
            .AsNoTracking()
            .Where(p => p.location_id == locationId).Select(p => new HolidayDto 
            {
                // Base
                Uuid = p.uuid,
                LocationId = p.location_id,
                IsActive = p.is_active,

                // extend_desc
                Day = p.day,
                Month = p.month,
                Year = p.year,
                Extend = p.extend,
                TypeMask = p.type_mask

            }).ToArrayAsync();

            return res;
      }

      public async Task<short> GetLowestUnassignedNumberAsync(int max)
      {
            if (max <= 0) return -1;

            var query = context.holiday
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
            return await context.holiday.AnyAsync(x => x.component_id == component);
      }

      public async Task<bool> IsAnyWithSameDataAsync(short day, short month, short year)
      {
            return await context.holiday.AnyAsync(u => u.day == day && u.month == month && u.year == year)
      }
}
