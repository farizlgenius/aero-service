using HIDAeroService.Entity.Interface;
using System.ComponentModel.DataAnnotations;

namespace HIDAeroService.Entity
{
    public sealed class Interval : IComponentId,IDatetime
    {
        [Key]
        public int Id { get; set; }
        public string Uuid { get; set; } = Guid.NewGuid().ToString();
        public bool IsActive { get; set; } = true;
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public short ComponentId { get; set; }
        public DaysInWeek Days { get; set; }
        public ICollection<TimeZoneInterval> TimeZoneIntervals { get; set; }
        public string DaysDesc { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        
    }
}
