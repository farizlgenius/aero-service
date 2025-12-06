using HIDAeroService.Entity.Interface;
using System.ComponentModel.DataAnnotations;

namespace HIDAeroService.Entity
{
    public class MonitorGroupList 
    {
        [Key]
        public int Id { get; set; }
        public short PointType { get; set; }
        public string PointTypeDesc { get; set; } = string.Empty;
        public short PointNumber { get; set; }
        public short MonitorGroupId { get; set; }
        public MonitorGroup MonitorGroup { get; set; }
    }
}
