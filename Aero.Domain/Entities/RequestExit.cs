using System;

namespace Aero.Domain.Entities;

public sealed class RequestExit : BaseDomain
{
    public short DeviceId { get; set; }
    public int ModuleId { get; set; }
    public short ModuleDriverId {get; set;}
    public int DoorId { get; set; }
    public short InputNo { get; set; }
    public short InputMode { get; set; }
    public short Debounce { get; set; }
    public short HoldTime { get; set; }
    public short MaskTimeZone { get; set; } = 0;

    public RequestExit() { }

    public RequestExit(
        short deviceId,
        int moduleId,
        short moduleDriverId,
        int doorId,
        short inputNo,
        short inputMode,
        short debounce,
        short holdTime,
        short maskTimeZone,
        int locationId,
        bool isActive = true) : base(locationId, isActive)
    {
        DeviceId = deviceId;
        ModuleId = moduleId;
        ModuleDriverId = moduleDriverId;
        DoorId = doorId;
        InputNo = inputNo;
        InputMode = inputMode;
        Debounce = debounce;
        HoldTime = holdTime;
        MaskTimeZone = maskTimeZone;
    }
}
