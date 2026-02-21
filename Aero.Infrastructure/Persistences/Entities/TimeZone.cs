

using Aero.Domain.Interface;
using Aero.Domain.Interfaces;

namespace Aero.Infrastructure.Persistences.Entities
{
    public sealed class TimeZone : BaseEntity,IDriverId
    {
        public short driver_id { get; set; }
        public string name { get; set; } = string.Empty;
        public short mode { get; set; }
        public string active_time { get; set; } = string.Empty;
        public string deactive_time { get; set; } = string.Empty;
        public ICollection<TimeZoneInterval> timezone_intervals { get; set; }
        public ICollection<AccessLevelComponent> access_level_components { get; set; }


    }
}
