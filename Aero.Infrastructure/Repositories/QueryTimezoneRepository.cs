using System;
using Aero.Application.DTOs;
using Aero.Application.Interfaces;
using Aero.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Aero.Infrastructure.Repositories;

public class QueryTimezoneRepository(AppDbContext context) : IQueryTimezoneRepository
{
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

  public Task<TimeZoneDto> GetByComponentIdAsync(short componentId)
  {
    throw new NotImplementedException();
  }

  public Task<IEnumerable<TimeZoneDto>> GetByLocationIdAsync(short locationId)
  {
    throw new NotImplementedException();
  }

  public Task<bool> IsAnyByComponet(short component)
  {
    throw new NotImplementedException();
  }
}
