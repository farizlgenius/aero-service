using Aero.Domain.Helpers;
using System;
using System.Net.Http.Headers;
using System.Reflection;

namespace Aero.Domain.Entities;

public class ControlPoint : BaseDomain
{
        public short DriverId {get; private set;}
       public string Name { get; private set; } = string.Empty;
        public int ModuleId { get; private set; }
        public string ModuleDetail { get; private set; } = string.Empty;
        public short OutputNo { get; private set; }
        public short RelayMode { get; private set; }
        public string RelayModeDetail { get; private set; } = string.Empty;
        public short OfflineMode { get; private set; }
        public string OfflineModeDetail { get; private set; } = string.Empty;
        public short DefaultPulse { get; private set; } = 1;
        public int DeviceId { get; private set; }

    public ControlPoint(short driverid,string name,int moduleid,string moduledetail,short outputno,short relaymode,string relaymodedetail,short offlinemode,string offlinemodedetail,short defaultpulse,int deviceId,int location,bool status) : base(location,status)
    {
        SetDriverId(driverid);
        SetName(name);
        SetModule(moduleid,moduledetail);
        SetOutput(outputno);
        SetRelay(relaymode,relaymodedetail);
        SetOffline(offlinemode,offlinemodedetail);
        SetDeviceId(deviceId);
        if (defaultpulse < 0) throw new ArgumentException("Default pulse invalid.");
    }

    private void SetId(int id)
    {
        if (id <= 0) throw new ArgumentException("Id invalid.");
    }

    private void SetDriverId(short driver)
    {
        if (driver <= 0) throw new ArgumentException("Driver id invalid.");
        DriverId = driver;
    }

    private void SetName(string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        if (!RegexHelper.IsValidName(name)) throw new ArgumentException("Name invalid.");
        Name = name;
    }

    private void SetModule(int moduleid,string moduledetail) 
    {
        if (moduleid < 0) throw new ArgumentException("Module Id invalid.");
        ArgumentException.ThrowIfNullOrWhiteSpace(moduledetail);
        ModuleId = moduleid;
        ModuleDetail = moduledetail;
    }

    private void SetOutput(short outputno)
    {
        if (outputno < 0) throw new ArgumentException("Output no invalid.");
        OutputNo = outputno;
    }

    private void SetRelay(short relaymode,string relaymodedetail)
    {
        if (relaymode < 0) throw new ArgumentException("Relay mode invalid.");
        ArgumentException.ThrowIfNullOrWhiteSpace(relaymodedetail);
        RelayMode = relaymode;
        RelayModeDetail = relaymodedetail;
    }

    private void SetOffline(short offlinemode, string offlinemodedetail)
    {
        if (offlinemode < 0) throw new ArgumentException("Offline mode invalid.");
        ArgumentException.ThrowIfNullOrWhiteSpace(offlinemodedetail);
        OfflineMode = offlinemode;
        OfflineModeDetail = offlinemodedetail;
    }

    private void SetDeviceId(int device)
    {
        if (device < 0) throw new ArgumentException("Device id invalid.");
        this.DeviceId = device;
    }
}
