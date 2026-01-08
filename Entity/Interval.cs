using HIDAeroService.Entity.Interface;
using System.ComponentModel.DataAnnotations;

namespace HIDAeroService.Entity
{
    public sealed class Interval : NoMacBaseEntity,IComponentId
    {
        public short component_id { get; set; }
        public DaysInWeek days { get; set; }
        public ICollection<TimeZoneInterval> timezone_intervals { get; set; }
        public string days_desc { get; set; } = string.Empty;
        public string start_time { get; set; } = string.Empty;
        public string end_time { get; set; } = string.Empty;
        
    }
}
