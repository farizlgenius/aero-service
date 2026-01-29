using System;

namespace Aero.Domain.Entities;

public sealed class DoorStatus
{
      public string Mac {get; set;} = string.Empty;
      public short AcrId {get; set;}
      public string AcrMode {get; set;} = string.Empty;
      public string AccessPointStatus {get; set;} = string.Empty;

}
