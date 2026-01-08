using HIDAeroService.Entity.Interface;
using System.ComponentModel.DataAnnotations;

namespace HIDAeroService.Entity
{
    public sealed class DaysInWeek : IDatetime
    {
        [Key]
        public int id { get; set; }
        public string uuid { get; set; } = Guid.NewGuid().ToString();
        public bool is_active { get; set; } = true;
        public DateTime created_date { get; set; }
        public DateTime updated_date { get; set; }
        public short component_id { get; set; }
        public Interval interval { get; set; }
        public bool sunday { get; set; }
        public bool monday { get; set; }
        public bool tuesday { get; set; }
        public bool wednesday { get; set; }
        public bool thursday { get; set; }
        public bool friday { get; set; }
        public bool saturday { get; set; }
    }
}
