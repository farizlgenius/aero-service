using System;
using Aero.Domain.Entities;

namespace Aero.Infrastructure.Mapper;

public sealed class NoMacBaseMapper
{
      public static void ToEf(Aero.Domain.Entities.NoMacBaseEntity from,Aero.Infrastructure.Data.Entities.NoMacBaseEntity to)
      {
            to.component_id = from.ComponentId;
            to.location_id = from.LocationId;
            to.is_active = from.IsActive;
            to.created_date = DateTime.UtcNow;
            to.updated_date = DateTime.UtcNow;
            to.is_active = true;
      }

      public static void Update(Aero.Domain.Entities.NoMacBaseEntity from,Aero.Infrastructure.Data.Entities.NoMacBaseEntity to)
      {
            to.is_active = from.IsActive;
            to.updated_date = DateTime.UtcNow;
      }
}
