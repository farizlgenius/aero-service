using System;

namespace Aero.Domain.Entities;

public sealed class AccessLevel : NoMacBaseEntity
{
      public string Name {get; set;} = string.Empty;

      public List<CreateUpdateAccessLevelComponent> Components {get; set;} = new List<CreateUpdateAccessLevelComponent>();
}
