using System.ComponentModel.DataAnnotations;

namespace Aero.Infrastructure.Persistences.Entities
{
    public class MonitorGroupList 
    {
        [Key]
        public int id { get; set; }
        public short point_type { get; set; }
        public string point_type_desc { get; set; } = string.Empty;
        public short point_number { get; set; }
        public short monitor_group_id { get; set; }
        public MonitorGroup monitor_group { get; set; }
    }
}
