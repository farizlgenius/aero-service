using System;
using Aero.Application.DTOs;
using Aero.Domain.Entities;

namespace Aero.Application.Mapper;

public sealed class OperatorMapper
{
      public static CreateOperator ToDomain(CreateOperatorDto dto)
      {
            return new CreateOperator
            {
                  ComponentId = dto.ComponentId,
                  Username = dto.Username,
                  Password = dto.Password,
                  Email = dto.Email,
                  Title = dto.Title,
                  FirstName = dto.FirstName,
                  MiddleName = dto.MiddleName,
                  LastName = dto.LastName,
                  Phone = dto.Phone,
                  ImagePath = dto.ImagePath,
                  RoleId = dto.RoleId,
                  LocationIds = dto.LocationIds,
            };
      }

}
