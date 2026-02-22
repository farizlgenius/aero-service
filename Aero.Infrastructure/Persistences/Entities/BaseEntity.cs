

using System.ComponentModel.DataAnnotations;
using Aero.Domain.Interface;


namespace Aero.Infrastructure.Persistences.Entities
{
    public class BaseEntity : IDatetime
    {
        [Key]
        public int id { get; set; }
        public int location_id { get; set; } = 1;
        public Location location { get; set; }
        public bool is_active { get; set; } = true;
        public DateTime created_date { get; set;} = DateTime.UtcNow;
        public DateTime updated_date { get; set; } = DateTime.UtcNow;

        public BaseEntity(int location_id)
        {
            this.location_id = location_id;
        }
    }
}
