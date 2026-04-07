using Aero.Domain.Helpers;
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
    public int LocationId { get; set; }

    public IdReport() { }

    public IdReport(short deviceId, short deviceVer, short softwareRevMajor, short softwareRevMinor, string firmware, int serialNumber, int ramSize, int ramFree, DateTimeOffset eSec, int dbMax, int dbActive, byte dipSwitchPowerup, byte dipSwitchCurrent, short scpId, short firmwareAdvisory, short scpIn1, short scpIn2, short nOemCode, byte configFlag, string mac, byte tlsStatus, byte operMode, short scpIn3, int cumulativeBldCnt, string ip, string port, int locationId)
    {
        DeviceId = deviceId;
        DeviceVer = deviceVer;
        SoftwareRevMajor = softwareRevMajor;
        SoftwareRevMinor = softwareRevMinor;
        Firmware = ValidateRequiredString(firmware, nameof(firmware));
        SerialNumber = serialNumber;
        RamSize = ramSize;
        RamFree = ramFree;
        ESec = eSec;
        DbMax = dbMax;
        DbActive = dbActive;
        DipSwitchPowerup = dipSwitchPowerup;
        DipSwitchCurrent = dipSwitchCurrent;
        ScpId = scpId;
        FirmwareAdvisory = firmwareAdvisory;
        ScpIn1 = scpIn1;
        ScpIn2 = scpIn2;
        NOemCode = nOemCode;
        ConfigFlag = configFlag;
        Mac = ValidateRequiredString(mac, nameof(mac));
        TlsStatus = tlsStatus;
        OperMode = operMode;
        ScpIn3 = scpIn3;
        CumulativeBldCnt = cumulativeBldCnt;
        Ip = ValidateRequiredString(ip, nameof(ip));
        Port = ValidateRequiredString(port, nameof(port));
        LocationId = locationId;
    }

    private static string ValidateRequiredString(string value, string field)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value, field);
        var trimmed = value.Trim();
        var sanitized = trimmed.Replace("-", string.Empty).Replace("_", string.Empty).Replace(".", string.Empty).Replace(":", string.Empty).Replace("/", string.Empty);
        if (!RegexHelper.IsValidName(trimmed) && !RegexHelper.IsValidOnlyCharAndDigit(sanitized))
        {
            throw new ArgumentException($"{field} invalid.", field);
        }

        return value;
    }
}
