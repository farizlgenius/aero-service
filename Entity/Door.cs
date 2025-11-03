using HIDAeroService.Entity.Interface;
using System.ComponentModel.DataAnnotations;

namespace HIDAeroService.Entity
{
    public sealed class Door : BaseEntity
    {
        public string Name { get; set; } = string.Empty;   
        public short AccessConfig { get; set; }
        public short PairDoorNo { get; set; }
        // Reader setting for Reader In / Reader Out
        public ICollection<Reader> Readers { get; set; }
        public short ReaderOutConfiguration { get; set; }

        // Strike setting for strike
        public short StrkComponentId { get; set; }
        public Strike Strk {  get; set; }

        //Sensor setting for sensor
        public short SensorComponentId { get; set; }
        public Sensor Sensor { get; set; }
        

        //Sensor setting for rex0 / rex1
        public ICollection<RequestExit>? RequestExits { get; set; }
        public short CardFormat { get; set; } = 255;
        public short AntiPassbackMode { get; set; }
        public short AntiPassBackIn { get; set; }
        public short AntiPassBackOut { get; set; }
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
        public short IlPBLongPress { get;set; }
        public short IlPBOutSio { get; set; }
        public short IlPBOutNum { get; set; }
        public short DfOfFilterTime { get; set; }
        public bool MaskHeldOpen { get; set; } = false;
        public bool MaskForceOpen { get; set; } = false;
        public ICollection<AccessLevelDoorTimeZone> AccessLevelDoorTimeZones { get; set; }

    }
}
