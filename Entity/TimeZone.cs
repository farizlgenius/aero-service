using HID.Aero.ScpdNet.Wrapper;
using HIDAeroService.Entity.Interface;
using System.ComponentModel.DataAnnotations;

namespace HIDAeroService.Entity
{
    public sealed class TimeZone : NoMacBaseEntity,IComponentId
    {
        public short ComponentId { get; set; }
        public string Name { get; set; } = string.Empty;
        public short Mode { get; set; }
        public string ActiveTime { get; set; } = string.Empty;
        public string DeactiveTime { get; set; } = string.Empty;
        public ICollection<TimeZoneInterval> TimeZoneIntervals { get; set; }
        public ICollection<AccessLevelDoorTimeZone> AccessLevelDoorTimeZones { get; set; }


    }
}
