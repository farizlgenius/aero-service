using System;

namespace Aero.Domain.Entities;

public sealed class AccessLevel : NoMacBaseEntity
{
      public string Name {get; set;} = string.Empty;
      public List<AccessLevelDoorTimezone> AccessLevelDoorTimezones { get; set;} = new List<AccessLevelDoorTimezone>();
}
