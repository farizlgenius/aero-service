using HIDAeroService.Entity.Interface;
using System.ComponentModel.DataAnnotations;

namespace HIDAeroService.Entity
{
    public sealed class AccessLevel : NoMacBaseEntity,IComponentId
    {
        public short ComponentId { get; set; }
        public string Name { get; set; } = string.Empty;
        public ICollection<AccessLevelDoorTimeZone> AccessLevelDoorTimeZones { get; set; }
        public ICollection<AccessLevelCredential> AccessLevelCredentials { get; set; }
    }
}
