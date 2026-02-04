using System;
using Aero.Domain.Entities;
using Aero.Infrastructure.Data.Entities;

namespace Aero.Infrastructure.Mapper;

public sealed class TimezoneMapper
{
      public static Aero.Infrastructure.Data.Entities.TimeZone ToEf(Timezone tz)
      {
            var res = new Aero.Infrastructure.Data.Entities.TimeZone();
            // Base 
            NoMacBaseMapper.ToEf(tz,res);
            res.component_id = tz.ComponentId;
            res.name = tz.Name;
            res.mode = tz.Mode;
            res.active_time = tz.ActiveTime;
            res.deactive_time = tz.DeactiveTime;
        res.timezone_intervals = tz.Intervals is null ? new List<TimeZoneInterval>() : tz.Intervals.Select(x => new Aero.Infrastructure.Data.Entities.TimeZoneInterval
        {
            timezone_id = tz.ComponentId,
            interval_id = x.ComponentId,
            created_date = DateTime.UtcNow,
            updated_date = DateTime.UtcNow
        }).ToArray();

            return res;
      }

      public static void Update(Aero.Infrastructure.Data.Entities.TimeZone en,Timezone timezone)
      {
            // Base 
            NoMacBaseMapper.Update(timezone,en);
            en.name = timezone.Name;
            en.mode = timezone.Mode;
            en.active_time = timezone.ActiveTime;
            en.deactive_time = timezone.DeactiveTime;
        en.timezone_intervals = timezone.Intervals is null ? new List<TimeZoneInterval>() :  timezone.Intervals.Select(x => new Aero.Infrastructure.Data.Entities.TimeZoneInterval
        {
            timezone_id = timezone.ComponentId,
            interval_id = x.ComponentId,
            created_date = DateTime.UtcNow,
            updated_date = DateTime.UtcNow
        }).ToArray();

    }

}
