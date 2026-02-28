using Aero.Domain.Interfaces;
using System;

namespace Aero.Domain.Entities;

public sealed class AccessLevelComponent()
{
    public short DriverId { get; private set; }
    public int DeviceId {get; private set;} 
    public int DoorId { get; private set; }
    public short AcrId { get; private set; }
    public short TimezoneId { get; private set; }

    public AccessLevelComponent(short driverId,int device,int doorId,short acrId,short timezoneId)
    {
        SetDriverId(driverId);
        SetDeviceId(device);
        SetDoorId(doorId);
        SetAcrId(acrId);
        SetTimezoneId(timezoneId);
    }

    public void SetDriverId(short driverid)
    {
        if (driverid <= 0) throw new ArgumentException("Access Level is invalid.");
        this.DriverId = driverid;
    }

    private void SetDeviceId(int deviceId)
    {
        if (deviceId <= 0) throw new ArgumentException("Device is invalid.");
        this.DeviceId = deviceId; 
    }

    private void SetDoorId(int doorid)
    {
        if (doorid <= 0) throw new ArgumentException("Door is invalid.");
        this.DoorId = doorid;
    }

    private void SetAcrId(short acrid)
    {
        if (acrid <= 0) throw new ArgumentException("ACR is invalid.");
        this.AcrId = acrid;
    }

    private void SetTimezoneId(short timezone)
    {
        if (timezone <= 0) throw new ArgumentException("Timezone is invalid.");
        this.TimezoneId = timezone;
    }
}
