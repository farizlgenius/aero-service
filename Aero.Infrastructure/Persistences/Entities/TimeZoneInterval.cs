using System.ComponentModel.DataAnnotations;
using Aero.Domain.Interface;

namespace Aero.Infrastructure.Persistences.Entities
{
    public sealed class TimeZoneInterval : IDatetime
    {
        [Key]
        public int id { get; set; }
        public bool is_active { get; set; } = true;
        public DateTime created_date { get; set; }
        public DateTime updated_date { get; set; }
        public short timezone_id {  get; set; }
        public TimeZone timezone { get; set; }
        public int interval_id { get; set; }
        public Interval interval { get; set; }
    }
}
