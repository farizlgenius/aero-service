using System;

namespace Aero.Domain.Entities;

public sealed class Sensor : BaseDomain
{
    public int DeviceId { get; set; }
    public short ModuleId { get; set; }
    public short DoorId { get; set; }
    public short InputNo { get; set; }
    public short InputMode { get; set; }
    public short Debounce { get; set; }
    public short HoldTime { get; set; }
    public short DcHeld { get; set; }

    public Sensor() { }

    public Sensor(
        int deviceId,
        short moduleId,
        short doorId,
        short inputNo,
        short inputMode,
        short debounce,
        short holdTime,
        short dcHeld,
        int locationId,
        bool isActive = true) : base(locationId, isActive)
    {
        DeviceId = deviceId;
        ModuleId = moduleId;
        DoorId = doorId;
        InputNo = inputNo;
        InputMode = inputMode;
        Debounce = debounce;
        HoldTime = holdTime;
        DcHeld = dcHeld;
    }
}
