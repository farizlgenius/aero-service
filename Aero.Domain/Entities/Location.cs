using System;
using Aero.Domain.Interface;

namespace Aero.Domain.Entities;

public class Location
{
      public short ComponentId { get; set; }
      public string Name { get; set; } = string.Empty;
      public string Description { get; set; } = string.Empty;
}
