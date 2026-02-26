using System;

namespace Aero.Domain.Entities;

public sealed class Procedure : BaseDomain
{
      public short DriverId {get; set;}
      public int TriggerId {get; set;}
      public string Name { get; set; } = string.Empty;
      public List<Action> Actions { get; set; } = new List<Action>();
}
