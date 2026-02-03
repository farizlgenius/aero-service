using System;
using System.Security.Cryptography.X509Certificates;
using Aero.Application.DTOs;
using Aero.Domain.Entities;

namespace Aero.Application.Mapper;

public sealed class LocationMapper
{
      public static Aero.Domain.Entities.Location ToDomain(LocationDto dto)
      {
            return new Aero.Domain.Entities.Location {
                  LocationName = dto.LocationName,
                  Description = dto.Description,
                  IsActive = dto.IsActive,
                  ComponentId = dto.ComponentId,
            };
      }

}
