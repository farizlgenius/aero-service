using System.ComponentModel.DataAnnotations;
using Aero.Domain.Interface;

namespace Aero.Infrastructure.Persistences.Entities
{
    public sealed class TimeZoneInterval : IDatetime
    {
        [Key]
        public int id { get; set; }
        public DateTime created_date { get; set; } = DateTime.UtcNow;
        public DateTime updated_date { get; set; } = DateTime.UtcNow;
        public int timezone_id { get; set; }
        public TimeZone timezone { get; set; }
        public int interval_id { get; set; }
        public Interval interval { get; set; }

        public TimeZoneInterval(int timezone_id, int interval_id)
        {
            this.timezone_id = timezone_id;
            this.interval_id = interval_id;
        }
    }
}
