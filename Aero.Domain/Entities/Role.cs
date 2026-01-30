using System;

namespace Aero.Domain.Entities;

public sealed class Role
{
      public short ComponentId { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<Feature> Features { get; set; } = new List<Feature>();

}
