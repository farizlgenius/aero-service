using System;

namespace Aero.Domain.Entities;

public sealed class Strike : BaseDomain
{
    public short DeviceId { get; set; }
    public int ModuleId { get; set; }
    public short ModuleDriverId {get; set;}
    public int DoorId { get; set; }
    public short OutputNo { get; set; }
    public short RelayMode { get; set; }
    public short OfflineMode { get; set; }
    public short StrkMax { get; set; }
    public short StrkMin { get; set; }
    public short StrkMode { get; set; }

    public Strike() { }

    public Strike(
        short deviceId,
        int moduleId,
        short moduleDriverId,
        int doorId,
        short outputNo,
        short relayMode,
        short offlineMode,
        short strkMax,
        short strkMin,
        short strkMode,
        int locationId,
        bool isActive = true) : base(locationId, isActive)
    {
        DeviceId = deviceId;
        ModuleId = moduleId;
        ModuleDriverId = moduleDriverId;
        DoorId = doorId;
        OutputNo = outputNo;
        RelayMode = relayMode;
        OfflineMode = offlineMode;
        StrkMax = strkMax;
        StrkMin = strkMin;
        StrkMode = strkMode;
    }
}
