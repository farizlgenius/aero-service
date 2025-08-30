using System.ComponentModel.DataAnnotations;

namespace HIDAeroService.Entity
{
    public sealed class ArAcr : ArBaseEntity
    {
        public string Name { get; set; }   
        public string ScpMac { get; set; }
        public short AcrNo { get; set; }
        public short AccessCfg { get; set; }
        public short RdrSio { get; set; }
        public short ReaderNo { get; set; }
        public short StrkSio { get; set; }
        public short StrkNo { get; set; }
        public short StrkMin { get; set; }
        public short StrkMax { get; set; }
        public short StrkMode { get; set; }
        public short SensorSio { get; set; }
        public short SensorNo { get; set; }
        public short DcHeld { get; set; }
        public short Rex1Sio { get; set; }
        public short Rex1No { get; set; }
        public short Rex2Sio { get; set; }
        public short Rex2No { get; set; }
        public short Rex1TzMask { get; set; }
        public short Rex2TzMask { get; set; }
        public short AlternateReaderSio { get; set; }
        public short AlternateReaderNo { get;set; }
        public short AlternateReaderSpec { get; set; }
        public short CdFormat { get; set; }
        public short ApbMode { get; set; }
        public short OfflineMode { get; set; }
        public short DefaultMode { get; set; }
        public short DoorMode { get; set; }
        public short DefaultLEDMode { get; set; }
        public short PreAlarm { get; set; }
        public short ApbDelay { get; set; }
        public short StrkT2 { get; set; }
        public short DcHeld2 { get; set; }
        public short StrkFollowPulse { get; set; }
        public short StrkFollowDelay { get; set; }
        public short NExtFeatureType { get; set; }
        public short IlPBSio { get; set; }
        public short IlPBNumber { get; set; }
        public short IlPBLongPress { get;set; }
        public short IlPBOutSio { get; set; }
        public short IlPBOutNum { get; set; }
        public short DfOfFilterTime { get; set; }

        
    }
}
