using System;

namespace Aero.Application.DTOs;

public sealed class CpStatus
{
      public string Mac {get; set;} = string.Empty;
      public int First {get; set;}
      public string Status {get; set;} = string.Empty;
}
