
using System.ComponentModel.DataAnnotations;
using Aero.Domain.Interface;


namespace Aero.Infrastructure.Persistences.Entities
{
    public sealed class Location : IDatetime
    {
        [Key]
        public int id { get; set; }
        public string name { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;

        // All HardwareComponent 
        public ICollection<Device> hardwares { get; set; }
        public ICollection<Module> modules { get; set; }
        public ICollection<ControlPoint> control_points { get; set; }
        public ICollection<MonitorPoint> monitor_points { get; set; }
        public ICollection<AccessLevel> accesslevels { get; set; }
        public ICollection<AccessArea> areas { get; set; }
        public ICollection<User> cardholders { get; set; }
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
        public ICollection<CardFormat> card_formats { get; set; }
        public ICollection<IdReport> idreports { get; set; }
        public ICollection<Role> roles { get; set; }
        public ICollection<CommandAudit> command_audit { get; set; }
        public ICollection<Company> companies { get; set; }
        public bool is_active { get; set; } = true;
        public DateTime created_date { get; set;} = DateTime.UtcNow;
        public DateTime updated_date { get; set; } = DateTime.UtcNow;

        public Location(){}
        public Location(Aero.Domain.Entities.Location data) 
        {
            this.name = data.Name;
            this.description = data.Description;
        }

        public void Update(Aero.Domain.Entities.Location data) 
        {
            this.name = data.Name;
            this.description = data.Description;
            this.updated_date = DateTime.UtcNow;
        }

    }
}
