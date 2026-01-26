using Aero.Domain.Interface;

namespace Aero.Infrastructure.Data.Entities
{
    public sealed class AccessLevel : NoMacBaseEntity,IComponentId
    {
        public string name { get; set; } = string.Empty;
        public ICollection<AccessLevelDoorTimeZone> accesslevel_door_timezones { get; set; }
        public ICollection<CardHolderAccessLevel> cardholder_accesslevel { get; set; }
    }
}
