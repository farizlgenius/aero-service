


using Aero.Domain.Interface;

namespace Aero.Infrastructure.Persistences.Entities
{
    public sealed class Interval : BaseEntity
    {
        public DaysInWeek days { get; set; }
        public ICollection<TimeZoneInterval> timezone_intervals { get; set; }
        public string days_desc { get; set; } = string.Empty;
        public string start_time { get; set; } = string.Empty;
        public string end_time { get; set; } = string.Empty;
        
    }
}
