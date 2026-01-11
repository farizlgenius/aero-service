using HID.Aero.ScpdNet.Wrapper;
using AeroService.Entity.Interface;
using System.ComponentModel.DataAnnotations;

namespace AeroService.Entity
{
    public sealed class TimeZone : NoMacBaseEntity,IComponentId
    {
        public short component_id { get; set; }
        public string name { get; set; } = string.Empty;
        public short mode { get; set; }
        public string active_time { get; set; } = string.Empty;
        public string deactive_time { get; set; } = string.Empty;
        public ICollection<TimeZoneInterval> timezone_intervals { get; set; }
        public ICollection<AccessLevelDoorTimeZone> accesslevel_door_timezones { get; set; }


    }
}
