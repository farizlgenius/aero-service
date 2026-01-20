using System;

namespace Aero.Application.Entities;

public sealed class Configurations
{
  public string ComponentName { get; set; } = string.Empty;
  public int nMismatchRecord { get; set; }
  public bool IsUpload { get; set; }
}
