using System;

namespace Aero.Domain.Entities;

public sealed class AccessLevel : NoMacBaseEntity
{
      public string Name {get; set;} = string.Empty;
      public List<AccessLevelComponent> Components {get; set;} = new List<AccessLevelComponent>();
}
