using System;
using Aero.Application.DTOs;
using Aero.Application.Interfaces;
using Aero.Domain.Entities;
using Aero.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Aero.Infrastructure.Repositories;

public class QTzRepository(AppDbContext context) : IQTzRepository
{
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
.Where(x => x.location_id == locationId)
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
.ToArrayAsync();

    return res;
  }

  public async Task<short> GetLowestUnassignedNumberAsync(int max,string mac)
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
    return await context.hardware.AnyAsync(x => x.component_id == component);
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
}
