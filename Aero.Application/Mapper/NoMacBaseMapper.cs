using System;
using Aero.Domain.Entities;

namespace Aero.Application.Mapper;

public sealed class NoMacBaseMapper
{
      public static void ToDomain(NoMacBaseEntity from,NoMacBaseEntity to)
      {
            to.ComponentId = from.ComponentId;
            to.HardwareName = from.HardwareName;
            to.LocationId = from.LocationId;
            to.IsActive = from.IsActive;
      }
}
