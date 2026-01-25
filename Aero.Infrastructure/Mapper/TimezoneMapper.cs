using System;
using Aero.Domain.Entities;

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
            
      }

}
