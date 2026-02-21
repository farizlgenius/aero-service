using Aero.Domain.Interface;
using Aero.Domain.Interfaces;

namespace Aero.Infrastructure.Persistences.Entities
{
    public sealed class AccessLevel : BaseEntity,IDriverId
    {
        public short driver_id { get; set; }
        public string name { get; set; } = string.Empty;
        public ICollection<AccessLevelComponent> components { get; set;}
        public ICollection<CardHolderAccessLevel> cardholder_access_levels { get; set;}
        
    }

}
