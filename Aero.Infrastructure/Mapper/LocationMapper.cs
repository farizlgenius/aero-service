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
            operator_locations = new List<OperatorLocation>
            {
                new OperatorLocation
                {   
                    location_id = data.ComponentId,
                    operator_id = 1
                },
            },

                  is_active = data.IsActive,
            created_date = DateTime.UtcNow,
            updated_date = DateTime.UtcNow
        };
      }

    public static void Update(Aero.Domain.Entities.Location from,Aero.Infrastructure.Data.Entities.Location to)
    {
        to.location_name = from.LocationName;
        to.description = from.Description;
        to.is_active = from.IsActive;
        to.updated_date = DateTime.UtcNow;
    }

}
