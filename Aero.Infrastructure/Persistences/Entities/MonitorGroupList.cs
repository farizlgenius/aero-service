using System.ComponentModel.DataAnnotations;

namespace Aero.Infrastructure.Persistences.Entities
{
    public class MonitorGroupList 
    {
        [Key]
        public int id { get; set; }
        public short point_type { get; set; }
        public string point_type_detail { get; set; } = string.Empty;
        public short point_number { get; set; }
        public int monitor_group_id { get; set; }
        public MonitorGroup monitor_group { get; set; }

        public MonitorGroupList(Aero.Domain.Entities.MonitorGroupList data) 
        {
            this.point_type = data.PointType;
            this.point_type_detail = data.PointTypeDetail;
            this.point_number = data.PointNumber;
            this.monitor_group_id = data.MonitorGroupId;
        }
    }
}
