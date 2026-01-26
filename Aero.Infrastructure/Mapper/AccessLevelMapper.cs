using System;
using Aero.Domain.Entities;

namespace Aero.Infrastructure.Mapper;

public sealed class AccessLevelMapper
{
      public static Aero.Infrastructure.Data.Entities.AccessLevel ToEf(CreateUpdateAccessLevel data)
      {
            var res = new Aero.Infrastructure.Data.Entities.AccessLevel();
            // Base 
            NoMacBaseMapper.ToEf(data,res);
            res.component_id = data.ComponentId;
            res.accesslevel_door_timezones =  data.CreateUpdateAccessLevelDoorTimeZone
            .Select(x => new Aero.Infrastructure.Data.Entities.AccessLevelDoorTimeZone
            {
                  accesslevel_id = data.ComponentId,
                  timezone_id = x.TimezoneId,
                  door_id = x.DoorId,
            }).ToList();

            return res;
      }

      public static void Update(Aero.Domain.Entities.CreateUpdateAccessLevel from,Aero.Infrastructure.Data.Entities.AccessLevel to)
      {
            // Base 
            NoMacBaseMapper.Update(from,to);
            to.name = from.Name;
            to.accesslevel_door_timezones = from.CreateUpdateAccessLevelDoorTimeZone.Select(x => new Aero.Infrastructure.Data.Entities.AccessLevelDoorTimeZone
            {
                  accesslevel_id = from.ComponentId,
                  timezone_id = x.TimezoneId,
                  door_id = x.DoorId
            }).ToArray();
      }

}
