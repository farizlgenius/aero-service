using HIDAeroService.Entity.Interface;
using System.ComponentModel.DataAnnotations;

namespace HIDAeroService.Entity
{
    public sealed class TimeZoneInterval : IDatetime
    {
        [Key]
        public int id { get; set; }
        public string uuid { get; set; } = Guid.NewGuid().ToString();
        public bool is_active { get; set; } = true;
        public DateTime created_date { get; set; }
        public DateTime updated_date { get; set; }
        public short timezone_id {  get; set; }
        public TimeZone timezone { get; set; }
        public short interval_id { get; set; }
        public Interval interval { get; set; }
    }
}
