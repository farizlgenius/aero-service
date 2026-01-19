using Aero.Infrastructure.Data.Interface;

namespace Aero.Infrastructure.Data.Entities
{
    public sealed class AccessLevel : NoMacBaseEntity,IComponentId
    {
        public short component_id { get; set; }
        public string name { get; set; } = string.Empty;
        public ICollection<AccessLevelDoorTimeZone> accessleve_door_timezones { get; set; }
        public ICollection<CardHolderAccessLevel> cardholder_accesslevel { get; set; }
    }
}
