

using Aero.Domain.Interface;

namespace Aero.Infrastructure.Data.Entities
{
    public sealed class TimeZone : NoMacBaseEntity
    {
        public short timezone_id { get; set; }
        public string name { get; set; } = string.Empty;
        public short mode { get; set; }
        public string active_time { get; set; } = string.Empty;
        public string deactive_time { get; set; } = string.Empty;
        public ICollection<TimeZoneInterval> timezone_intervals { get; set; }
        public ICollection<AccessLevelComponent> access_level_components { get; set; }


    }
}
