using HIDAeroService.Entity.Interface;
using System.ComponentModel.DataAnnotations;

namespace HIDAeroService.Entity
{
    public class NoMacBaseEntity : IDatetime
    {
        [Key]
        public int id { get; set; }
        public string uuid { get; set; } = Guid.NewGuid().ToString();
        public short location_id { get; set; } = 1;
        public Location location { get; set; }
        public bool is_active { get; set; } = true;
        public DateTime created_date { get; set; }
        public DateTime updated_date { get; set; }
    }
}
