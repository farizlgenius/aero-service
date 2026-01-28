using System;

namespace Aero.Domain.Entities;

public sealed class CreateUpdateAccessLevelComponent
{
      public string Mac {get; set;} = string.Empty;
      public List<CreateUpdateAccessLevelDoorComponent> DoorComponents { get; set;} = new List<CreateUpdateAccessLevelDoorComponent>();
}
