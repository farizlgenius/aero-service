
using System.ComponentModel.DataAnnotations;
using Aero.Domain.Interface;


namespace Aero.Infrastructure.Data.Entities
{
    public sealed class Location : IComponentId,IDatetime
    {
        [Key]
        public int id { get; set; }
        public string uuid { get; set; } = Guid.NewGuid().ToString();
        public short component_id { get; set; }
        public string location_name { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;

        // All HardwareComponent 
        public ICollection<Hardware> hardwares { get; set; }
        public ICollection<Module> modules { get; set; }
        public ICollection<ControlPoint> control_points { get; set; }
        public ICollection<MonitorPoint> monitor_points { get; set; }
        public ICollection<AccessLevel> accesslevels { get; set; }
        public ICollection<Area> areas { get; set; }
        public ICollection<CardHolder> cardholders { get; set; }
        public ICollection<Door> doors { get; set; }
        public ICollection<MonitorGroup> monitor_groups { get; set; }
        public ICollection<OperatorLocation> operator_locations { get; set; }
        public ICollection<Transaction> transactions { get; set; }
        public ICollection<Credential> credentials { get; set; }
        public ICollection<Holiday> holidays { get; set; }
        public ICollection<Reader> readers { get; set; }
        public ICollection<RequestExit> request_exits { get; set; }
        public ICollection<Sensor> sensors { get; set; }
        public ICollection<Strike> strikes { get; set; }
        public ICollection<Procedure> procedures { get; set; }
        public ICollection<Action> actions { get; set; }
        public ICollection<Trigger> triggers { get; set; }
        public ICollection<Interval> intervals { get; set; }
        public ICollection<TimeZone> timezones { get; set; }

        public bool is_active { get; set; } = true;
        public DateTime created_date { get; set; }
        public DateTime updated_date { get; set; }

    }
}
