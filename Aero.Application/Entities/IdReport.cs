using System;

namespace Aero.Application.Entities;

public sealed class IdReport
{
  public short ComponentId { get; set; }
  public int SerialNumber { get; set; }
  public string Mac { get; set; } = string.Empty;
  public string Ip { get; set; } = string.Empty;
  public string Port { get; set; } = string.Empty;
  public string Firmware { get; set; } = string.Empty;
  public short HardwareType { get; set; }
  public string HardwareTypeDescription { get; set; } = string.Empty;

}
