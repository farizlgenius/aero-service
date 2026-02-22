using System;
using Aero.Domain.Entities;
using Aero.Infrastructure.Persistences.Entities;

namespace Aero.Infrastructure.Mapper;

public sealed class AccessLevelMapper
{
      public static Aero.Infrastructure.Persistences.Entities.AccessLevel ToDb(Aero.Domain.Entities.AccessLevel data)
      {

            var res = new Aero.Infrastructure.Persistences.Entities.AccessLevel(
                data.DriverId,
                data.Name,
                data.Components.Select(x => new Persistences.Entities.AccessLevelComponent(x.DriverId,x.Mac,x.DoorId,x.AcrId,x.TimezoneId)).ToList(),
                data.LocationId
                );
            return res;
      }

      public static void Update(Aero.Domain.Entities.AccessLevel from,Aero.Infrastructure.Persistences.Entities.AccessLevel to)
      {
            // Base 
            NoMacBaseMapper.Update(from,to);
            to.name = from.Name;
            to.components = from.Components.Select(x => new Persistences.Entities.AccessLevelComponent
            {
                alvl_id = x.AlvlId,
                door_id= x.DoorId,
                  mac = x.Mac,
                acr_id = x.AcrId,
                timezone_id = x.TimezoneId
            }).ToList();
      }

}
