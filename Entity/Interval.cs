using HIDAeroService.Entity.Interface;

namespace HIDAeroService.Entity
{
    public sealed class Interval : NoMacBaseEntity,IComponentId
    {
        public short ComponentId { get; set; }
        public DaysInWeek Days { get; set; }
        public ICollection<TimeZoneInterval> TimeZoneIntervals { get; set; }
        public string DaysDesc { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        
    }
}
