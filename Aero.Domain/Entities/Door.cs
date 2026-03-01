using Aero.Domain.Enums;
using Aero.Domain.Helpers;
using System;

namespace Aero.Domain.Entities;

public sealed class Door : BaseDomain
{
    public int Id {get; set;}
    public short DriverId { get; set; }
    public string Name { get; set; } = string.Empty;
    public short AccessConfig { get; set; }
    public short PairDoorNo { get; set; }
    public DoorDirection Direction { get; set; }
    public int DeviceId { get; set; }
    public List<Reader> Readers { get; set; } = new List<Reader>();
    public short ReaderOutConfiguration { get; set; }
    public Strike Strk { get; set; } 
    public Sensor Sensor { get; set; }
    public List<RequestExit> RequestExits { get; set; } = new List<RequestExit>();
    public short CardFormat { get; set; }
    public short AntiPassbackMode { get; set; }
    public short AntiPassBackIn { get; set; }
    public short AreaInId { get; set; }
    public short AntiPassBackOut { get; set; }
    public short AreaOutId { get; set; }
    public short SpareTags { get; set; }
    public short AccessControlFlags { get; set; }
    public short Mode { get; set; }
    public string ModeDesc { get; set; } = string.Empty;
    public short OfflineMode { get; set; }
    public string OfflineModeDesc { get; set; } = string.Empty;
    public short DefaultMode { get; set; }
    public string DefaultModeDesc { get; set; } = string.Empty;
    public short DefaultLEDMode { get; set; }
    public short PreAlarm { get; set; }
    public short AntiPassbackDelay { get; set; }
    public short StrkT2 { get; set; }
    public short DcHeld2 { get; set; }
    public short StrkFollowPulse { get; set; }
    public short StrkFollowDelay { get; set; }
    public short nExtFeatureType { get; set; }
    public short IlPBSio { get; set; }
    public short IlPBNumber { get; set; }
    public short IlPBLongPress { get; set; }
    public short IlPBOutSio { get; set; }
    public short IlPBOutNum { get; set; }
    public short DfOfFilterTime { get; set; }
    public bool MaskHeldOpen { get; set; }
    public bool MaskForceOpen { get; set; }
    public List<AccessLevelComponent> AccessLevelComponents { get; set; } = new List<AccessLevelComponent>();

    public Door(int Id,short driverId, string name, short accessConfig, short pairDoorNo, DoorDirection direction, int deviceId, List<Reader> readers, short readerOutConfiguration, Strike strk, Sensor sensor, List<RequestExit> requestExits, short cardFormat, short antiPassbackMode, short antiPassBackIn, short areaInId, short antiPassBackOut, short areaOutId, short spareTags, short accessControlFlags, short mode, string modeDesc, short offlineMode, string offlineModeDesc, short defaultMode, string defaultModeDesc, short defaultLedMode, short preAlarm, short antiPassbackDelay, short strkT2, short dcHeld2, short strkFollowPulse, short strkFollowDelay, short nExtFeatureType, short ilPBSio, short ilPBNumber, short ilPBLongPress, short ilPBOutSio, short ilPBOutNum, short dfOfFilterTime, bool maskHeldOpen, bool maskForceOpen, List<AccessLevelComponent> accessLevelComponents)
    {
        this.Id = Id;
        DriverId = driverId;
        Name = ValidateRequiredString(name, nameof(name));
        AccessConfig = accessConfig;
        PairDoorNo = pairDoorNo;
        Direction = direction;
        DeviceId = deviceId;
        Readers = readers ?? throw new ArgumentNullException(nameof(readers));
        ReaderOutConfiguration = readerOutConfiguration;
        Strk = strk;
        Sensor = sensor;
        RequestExits = requestExits;
        CardFormat = cardFormat;
        AntiPassbackMode = antiPassbackMode;
        AntiPassBackIn = antiPassBackIn;
        AreaInId = areaInId;
        AntiPassBackOut = antiPassBackOut;
        AreaOutId = areaOutId;
        SpareTags = spareTags;
        AccessControlFlags = accessControlFlags;
        Mode = mode;
        ModeDesc = ValidateRequiredString(modeDesc, nameof(modeDesc));
        OfflineMode = offlineMode;
        OfflineModeDesc = ValidateRequiredString(offlineModeDesc, nameof(offlineModeDesc));
        DefaultMode = defaultMode;
        DefaultModeDesc = ValidateRequiredString(defaultModeDesc, nameof(defaultModeDesc));
        DefaultLEDMode = defaultLedMode;
        PreAlarm = preAlarm;
        AntiPassbackDelay = antiPassbackDelay;
        StrkT2 = strkT2;
        DcHeld2 = dcHeld2;
        StrkFollowPulse = strkFollowPulse;
        StrkFollowDelay = strkFollowDelay;
        this.nExtFeatureType = nExtFeatureType;
        IlPBSio = ilPBSio;
        IlPBNumber = ilPBNumber;
        IlPBLongPress = ilPBLongPress;
        IlPBOutSio = ilPBOutSio;
        IlPBOutNum = ilPBOutNum;
        DfOfFilterTime = dfOfFilterTime;
        MaskHeldOpen = maskHeldOpen;
        MaskForceOpen = maskForceOpen;
        AccessLevelComponents = accessLevelComponents ?? throw new ArgumentNullException(nameof(accessLevelComponents));
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
