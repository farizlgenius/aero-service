using HIDAeroService.Entity.Interface;
using System.ComponentModel.DataAnnotations;

namespace HIDAeroService.Entity
{
    public sealed class TimeZoneInterval : IDatetime
    {
        [Key]
        public int Id { get; set; }
        public string Uuid { get; set; } = Guid.NewGuid().ToString();
        public bool IsActive { get; set; } = true;
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public short TimeZoneId {  get; set; }
        public TimeZone TimeZone { get; set; }
        public short IntervalId { get; set; }
        public Interval Interval { get; set; }
    }
}
