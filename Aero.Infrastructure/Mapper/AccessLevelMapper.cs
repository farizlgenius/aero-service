using System;
using Aero.Domain.Entities;
using Aero.Infrastructure.Data.Entities;

namespace Aero.Infrastructure.Mapper;

public sealed class AccessLevelMapper
{
      public static Aero.Infrastructure.Data.Entities.AccessLevel ToEf(CreateUpdateAccessLevel data)
      {
            var res = new Aero.Infrastructure.Data.Entities.AccessLevel();
            // Base 
            NoMacBaseMapper.ToEf(data,res);
            res.name = data.Name;
            res.component_id = data.ComponentId;
            res.components =  data.Components
            .Select(x => new AccessLevelComponent
            {
                  mac = x.Mac,
                  door_component = x.DoorComponents.Select(x => new AccessLevelDoorComponent
                  {
                        door_id = x.DoorId,
                        acr_id = x.AcrId,
                        timezone_id = x.TimezoneId
                  }).ToList()
            }).ToList();

            return res;
      }

      public static void Update(Aero.Domain.Entities.CreateUpdateAccessLevel from,Aero.Infrastructure.Data.Entities.AccessLevel to)
      {
            // Base 
            NoMacBaseMapper.Update(from,to);
            to.name = from.Name;
            to.components = from.Components.Select(x => new AccessLevelComponent
            {
                  mac = x.Mac,
                  door_component = x.DoorComponents.Select(a => new AccessLevelDoorComponent
                  {
                        acr_id = a.AcrId,
                        timezone_id = a.TimezoneId
                  }).ToList()
            }).ToList();
      }

}
