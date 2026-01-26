using System;

namespace Aero.Domain.Entities;

public sealed class CreateUpdateAccessLevel : NoMacBaseEntity
{
      public string Name { get; set; } = string.Empty;
        public List<CreateUpdateAccessLevelDoorTimeZone> CreateUpdateAccessLevelDoorTimeZone { get; set; } = new List<CreateUpdateAccessLevelDoorTimeZone>();
}
