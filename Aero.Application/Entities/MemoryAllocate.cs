using System;

namespace Aero.Application.Entities;

public sealed class MemoryAllocate
{
  public string Mac { get; set; } = string.Empty;
  public Memory Memory { get; set; } = new Memory();

}
