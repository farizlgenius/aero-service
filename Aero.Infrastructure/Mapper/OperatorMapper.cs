using System;
using System.Data.Common;
using Aero.Application.DTOs;
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

      public static void Update(CreateOperatorDto from,Aero.Infrastructure.Data.Entities.Operator to)
      {
            to.component_id = from.ComponentId;
            to.user_id = from.UserId;
            to.user_name = from.Username;
            to.email = from.Email;
            to.title = from.Title;
            to.first_name = from.FirstName;
            to.middle_name = from.MiddleName;
            to.last_name = from.LastName;
            to.phone = from.Phone;
            to.image_path = from.ImagePath;
            to.role_id = from.RoleId;
            to.operator_locations = from.LocationIds.Select(x => new OperatorLocation
            {
                  location_id = x,
                  operator_id = from.ComponentId
            }).ToList();
            to.updated_date = DateTime.UtcNow;

      }

}
