using HIDAeroService.Entity.Interface;
using System.ComponentModel.DataAnnotations;

namespace HIDAeroService.Entity
{
    public sealed class AccessLevel : NoMacBaseEntity,IComponentId
    {
        public short component_id { get; set; }
        public string name { get; set; } = string.Empty;
        public ICollection<AccessLevelDoorTimeZone> accessleve_door_timezones { get; set; }
        public ICollection<CardHolderAccessLevel> cardholder_accesslevel { get; set; }
    }
}
