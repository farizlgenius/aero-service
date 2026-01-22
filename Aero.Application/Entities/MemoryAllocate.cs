using System;

namespace Aero.Application.Entities;

public sealed class MemoryAllocate
{
  public string Mac { get; set; } = string.Empty;
  public List<Memory> Memories { get; set; } = new List<Memory>();

}
