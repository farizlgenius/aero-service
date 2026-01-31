using System;

namespace Aero.Domain.Entities;

public sealed class Procedure : BaseEntity
{
      public short ProcId {get; set;}
      public string Name { get; set; } = string.Empty;
      public List<Action> Actions { get; set; } = new List<Action>();
}
