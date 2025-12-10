using HIDAeroService.Entity.Interface;
using System.ComponentModel.DataAnnotations;

namespace HIDAeroService.Entity
{
    public sealed class Location : IComponentId,IDatetime
    {
        [Key]
        public int Id { get; set; }
        public string Uuid { get; set; } = Guid.NewGuid().ToString();
        public short ComponentId { get; set; }
        public string LocationName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        // All Component 
        public ICollection<Hardware> Hardwares { get; set; }
        public ICollection<Module> Modules { get; set; }
        public ICollection<ControlPoint> ControlPoints { get; set; }
        public ICollection<MonitorPoint> MonitorPoints { get; set; }
        public ICollection<AccessLevel> AccessLevels { get; set; }
        public ICollection<AccessArea> AccessAreas { get; set; }
        public ICollection<CardHolder> CardHolders { get; set; }
        public ICollection<Door> Doors { get; set; }
        public ICollection<MonitorGroup> MonitorPointsGroup { get; set; }
        public ICollection<OperatorLocation> OperatorLocations { get; set; }
        public ICollection<Transaction> Events { get; set; }
        public ICollection<AeroStructureStatus> AeroStructureStatuses { get; set; }
        public ICollection<Credential> Credentials { get; set; }
        public ICollection<Holiday> Holidays { get; set; }
        public ICollection<Reader> Readers { get; set; }
        public ICollection<RequestExit> RequestExits { get; set; }
        public ICollection<Sensor> Sensors { get; set; }
        public ICollection<Strike> Strikes { get; set; }
        public ICollection<Procedure> Procedures { get; set; }
        public ICollection<Action> Actions { get; set; }
        public ICollection<Trigger> Triggers { get; set; }
        public ICollection<Interval> Intervals { get; set; }
        public ICollection<TimeZone> TimeZones { get; set; }

        public bool IsActive { get; set; } = true;
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

    }
}
