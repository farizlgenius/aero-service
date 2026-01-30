using System;
using System.Data.Common;
using Aero.Infrastructure.Data.Entities;

namespace Aero.Infrastructure.Mapper;

public sealed class OperatorMapper
{
      public static Aero.Infrastructure.Data.Entities.Operator ToEf(Aero.Domain.Entities.CreateOperator data)
      {
            return new Aero.Infrastructure.Data.Entities.Operator
            {
                  component_id = data.ComponentId,
                  user_id = data.UserId,
                  user_name = data.Username,
                  password = data.Password,
                  email = data.Email,
                  title = data.Title,
                  first_name = data.FirstName,
                  middle_name = data.MiddleName,
                  last_name = data.LastName,
                  phone = data.Phone,
                  image_path = data.ImagePath,
                  role_id = data.RoleId,
                  is_active = true,
                  operator_locations = data.LocationIds.Select(x => new OperatorLocation
                  {
                        location_id = x,
                        operator_id = data.ComponentId
                  }).ToList(),
                  created_date = DateTime.UtcNow,
                  updated_date = DateTime.UtcNow

            };
      }

}
