using System;
using Aero.Domain.Enums;

namespace Aero.Domain.Entities;

public sealed class Reader : BaseDomain
{
    public short ModuleId { get; set; }
    public short DoorId { get; set; }
    public short ReaderNo { get; set; }
    public short DataFormat { get; set; } = 0x01;
    public short KeypadMode { get; set; } = 2;
    public short LedDriveMode { get; set; }
    public bool OsdpFlag { get; set; }
    public short OsdpBaudrate { get; set; } = 0x01;
    public short OsdpDiscover { get; set; } = 0x08;
    public short OsdpTracing { get; set; } = 0x10;
    public short OsdpAddress { get; set; }
    public short OsdpSecureChannel { get; set; }
    public int DeviceId { get; set; }

    public Reader() { }

    public Reader(
        short moduleId,
        short doorId,
        short readerNo,
        short dataFormat,
        short keypadMode,
        short ledDriveMode,
        bool osdpFlag,
        short osdpBaudrate,
        short osdpDiscover,
        short osdpTracing,
        short osdpAddress,
        short osdpSecureChannel,
        int deviceId,
        int locationId,
        bool isActive = true) : base(locationId, isActive)
    {
        ModuleId = moduleId;
        DoorId = doorId;
        ReaderNo = readerNo;
        DataFormat = dataFormat;
        KeypadMode = keypadMode;
        LedDriveMode = ledDriveMode;
        OsdpFlag = osdpFlag;
        OsdpBaudrate = osdpBaudrate;
        OsdpDiscover = osdpDiscover;
        OsdpTracing = osdpTracing;
        OsdpAddress = osdpAddress;
        OsdpSecureChannel = osdpSecureChannel;
        DeviceId = deviceId;
    }
}
