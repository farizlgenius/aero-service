using Aero.Domain.Interface;

namespace Aero.Infrastructure.Data.Entities
{
    public sealed class AccessLevel : NoMacBaseEntity
    {
        public string name { get; set; } = string.Empty;
        public ICollection<AccessLevelComponent> components { get; set;}
        public ICollection<CardHolderAccessLevel> cardholder_access_levels { get; set;}
        
    }

}
