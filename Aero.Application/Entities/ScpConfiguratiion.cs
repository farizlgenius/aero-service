using System;

namespace Aero.Application.Entities;

public sealed class ScpConfiguratiion
{
  public string Mac { get; set; } = string.Empty;
  public short LocationId { get; set; }

  public List<Configurations> Configurations { get; set; } = new List<Configurations>();

}
