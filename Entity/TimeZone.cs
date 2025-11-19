using HID.Aero.ScpdNet.Wrapper;
using HIDAeroService.Entity.Interface;
using System.ComponentModel.DataAnnotations;

namespace HIDAeroService.Entity
{
    public sealed class TimeZone : IComponentId,IDatetime
    {
        [Key]
        public int Id { get; set; }
        public string Uuid { get; set; } = Guid.NewGuid().ToString();
        public short ComponentId { get; set; }
        public string Name { get; set; }
        public short Mode { get; set; }
        public string ActiveTime { get; set; } = string.Empty;
        public string DeactiveTime { get; set; } = string.Empty;
        public ICollection<TimeZoneInterval> TimeZoneIntervals { get; set; }
        public ICollection<AccessLevelDoorTimeZone> AccessLevelDoorTimeZones { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }


    }
}
