using System;

namespace Aero.Domain.Entities;

public sealed class IdReport
{
      public short DeviceId { get; set; }
      public short DeviceVer { get; set; }
      public short SoftwareRevMajor { get; set; }
      public short SoftwareRevMinor { get; set; }
      public string Firmware { get; set; } = string.Empty;
      public int SerialNumber { get; set; }
      public int RamSize { get; set; }
      public int RamFree { get; set; }
      public DateTimeOffset ESec { get; set; }
      public int DbMax { get; set; }
      public int DbActive { get; set; }
      public byte DipSwitchPowerup { get; set; }
      public byte DipSwitchCurrent { get; set; }
      public short ScpId { get; set; }
      public short FirmwareAdvisory { get; set; }
      public short ScpIn1 { get; set; }
      public short ScpIn2 { get; set; }
      public short NOemCode { get; set; }
      public byte ConfigFlag { get; set; }
      public string Mac { get; set; } = string.Empty;
      public byte TlsStatus { get; set; }
      public byte OperMode { get; set; }
      public short ScpIn3 { get; set; }
      public int CumulativeBldCnt { get; set; }
      public string Ip { get; set; } = string.Empty;
      public string Port { get; set; } = string.Empty;

}
