using Aero.Domain.Interfaces;
using System;

namespace Aero.Domain.Entities;

public sealed class AccessLevelComponent
{
    public short DriverId { get; private set; }
    public string Mac {get; private set;} = string.Empty;
    public int DoorId { get; private set; }
    public short AcrId { get; private set; }
    public short TimezoneId { get; private set; }

    public AccessLevelComponent() { }

    public AccessLevelComponent(short driverId,string mac,int doorId,short acrId,short timezoneId)
    {
        SetDriverId(driverId);
        SetMac(mac);
        SetDoorId(doorId);
        SetAcrId(acrId);
        SetTimezoneId(timezoneId);
    }

    private void SetDriverId(short driverid)
    {
        if (driverid <= 0) throw new ArgumentException("Access Level is invalid.");
        this.DriverId = driverid;
    }

    private void SetMac(string mac)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(mac);
        this.Mac = mac; 
    }

    private void SetDoorId(short doorid)
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
