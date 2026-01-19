using System;

namespace Aero.Domain.Entities;

public sealed class Hardware : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public int HardwareType { get; set; }
    public string HardwareTypeDescription { get; set; } = string.Empty;
    public string Ip { get; set; } = string.Empty;
    public string Firmware { get; set; } = string.Empty;
    public string Port { get; set; } = string.Empty;
    public List<Module> Modules { get; set; }
    public string SerialNumber { get; set; } = string.Empty;
    public bool IsUpload { get; set; } = false;
    public bool IsReset { get; set; } = false;
    public bool PortOne { get; set; } = false;
    public short ProtocolOne { get; set; }
    public string ProtocolOneDescription { get; set; } = string.Empty;
    public short BaudRateOne { get; set; }
    public bool PortTwo { get; set; } = false;
    public short ProtocolTwo { get; set; }
    public string ProtocolTwoDescription { get; set; } = string.Empty;
    public short BaudRateTwo { get; set; }
    public DateTime LastSync { get; set; }
}
