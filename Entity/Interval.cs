using HIDAeroService.Entity.Interface;
using System.ComponentModel.DataAnnotations;

namespace HIDAeroService.Entity
{
    public sealed class Interval : NoMacBaseEntity,IComponentId
    {
        public short ComponentId { get; set; }
        public DaysInWeek Days { get; set; }
        public ICollection<TimeZoneInterval> TimeZoneIntervals { get; set; }
        public string DaysDesc { get; set; } = string.Empty;
        public string StartTime { get; set; } = string.Empty;
        public string EndTime { get; set; } = string.Empty;
        
    }
}
