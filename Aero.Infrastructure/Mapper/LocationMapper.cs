using System;
using Aero.Infrastructure.Data.Entities;

namespace Aero.Infrastructure.Mapper;

public sealed class LocationMapper
{
      public static Location ToEf(Aero.Domain.Entities.Location data)
      {
            return new Location
            {
                  component_id = data.ComponentId,
                  location_name = data.LocationName,
                  description = data.Description,

                  is_active = data.IsActive,
                  created_date = DateTime.UtcNow,
                  updated_date = DateTime.UtcNow
            };
      }

}
