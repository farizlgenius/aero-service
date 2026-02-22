using System;
using Aero.Application.DTOs;

namespace Aero.Infrastructure.Mapper;

public sealed class MacBaseMapper
{
      public static void ToEf(Aero.Domain.Entities.BaseDomain from,Aero.Infrastructure.Data.Entities.BaseEntity to)
      {
            to.component_id = from.ComponentId;
            to.mac = from.Mac;
            to.location_id = from.LocationId;
            to.is_active = from.IsActive;
            to.created_date = DateTime.UtcNow;
            to.updated_date = DateTime.UtcNow;
      }

      public static void Update(Aero.Domain.Entities.BaseDomain from,Aero.Infrastructure.Data.Entities.BaseEntity to)
      {
            to.is_active = from.IsActive;
            to.mac = from.Mac;
            to.updated_date = DateTime.UtcNow;
      }
}
