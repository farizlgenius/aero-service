using Aero.Domain.Enums;
using System;

namespace Aero.Domain.Entities;

public sealed class Door : BaseDomain
{
      public short DriverId { get; set; }
      public string Name { get; set; } = string.Empty;
      public short AccessConfig { get; set; }
      public short PairDoorNo { get; set; }
    public DoorDirection Direction { get; set; }
    public int DeviceId { get; set; } 
      // Reader setting for Reader In
      public List<Reader> Readers { get; set; }
      public short ReaderOutConfiguration { get; set; }

      // Strike setting for strike
      public Strike? Strk { get; set; }

      //sensor setting for sensor
      public Sensor? Sensor { get; set; }
      public List<RequestExit>? RequestExits { get; set; }
      public short CardFormat { get; set; }
      public short AntiPassbackMode { get; set; }
      public short AntiPassBackIn { get; set; }
      public short AreaInId {get; set;}
      public short AntiPassBackOut { get; set; }
      public short AreaOutId {get; set;}
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
      public bool MaskHeldOpen { get; set; } = false;
      public bool MaskForceOpen { get; set; } = false;
    public List<AccessLevelComponent> AccessLevelComponents { get; set; }

}
