using System;
using Aero.Domain.Entities;
using Aero.Infrastructure.Data.Entities;

namespace Aero.Infrastructure.Mapper;

public sealed class AccessLevelMapper
{
      public static Aero.Infrastructure.Data.Entities.AccessLevel ToEf(Aero.Domain.Entities.AccessLevel data)
      {
            var res = new Aero.Infrastructure.Data.Entities.AccessLevel();
            // Base 
            NoMacBaseMapper.ToEf(data,res);
            res.name = data.Name;
            res.component_id = data.ComponentId;
        
            res.components =  data.Components
            .Select(x => new Data.Entities.AccessLevelComponent
            {
                alvl_id = x.AlvlId,
                door_id = x.DoorId,
                acr_id = x.AcrId,
                timezone_id = x.TimezoneId
            }).ToList();

            return res;
      }

      public static void Update(Aero.Domain.Entities.AccessLevel from,Aero.Infrastructure.Data.Entities.AccessLevel to)
      {
            // Base 
            NoMacBaseMapper.Update(from,to);
            to.name = from.Name;
            to.components = from.Components.Select(x => new Data.Entities.AccessLevelComponent
            {
                alvl_id = x.AlvlId,
                door_id= x.DoorId,
                  mac = x.Mac,
                acr_id = x.AcrId,
                timezone_id = x.TimezoneId
            }).ToList();
      }

}
