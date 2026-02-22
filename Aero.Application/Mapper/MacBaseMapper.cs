using System;
using Aero.Domain.Entities;

namespace Aero.Application.Mapper;

public sealed class MacBaseMapper 
{
      public static void ToDomain(BaseDomain from,BaseDomain to)
      {
            to.ComponentId = from.ComponentId;
            to.HardwareName = from.HardwareName;
            to.LocationId = from.LocationId;
            to.Mac = from.Mac;
            to.IsActive = from.IsActive;
      }
}
