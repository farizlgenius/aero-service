using System;

namespace Aero.Domain.Entities;

public sealed class Password
{
      public string Username { get; set; } = string.Empty;
      public string Old { get; set; } = string.Empty;
      public string New { get; set; } = string.Empty;
      public string Con { get; set; } = string.Empty;
}
