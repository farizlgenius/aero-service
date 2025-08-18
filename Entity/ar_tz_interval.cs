using System.ComponentModel.DataAnnotations;

namespace HIDAeroService.Entity
{
    public sealed class ar_tz_interval
    {
        [Key]
        public int id { get; set; }
        public short tz_number { get; set; }
        public short intervals_number { get; set; }
        public short i_days { get; set; }
        public string i_start { get; set; }
        public string i_end { get; set; }
    }
}
