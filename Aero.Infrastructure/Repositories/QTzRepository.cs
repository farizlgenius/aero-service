using System;
using Aero.Application.DTOs;
using Aero.Application.Interfaces;
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

  public async Task<short> GetLowestUnassignedNumberAsync(int max)
  {
    throw new NotImplementedException();
  }

  public async Task<bool> IsAnyByComponentId(short component)
  {
    return await context.hardware.AnyAsync(x => x.component_id == component);
  }
}
