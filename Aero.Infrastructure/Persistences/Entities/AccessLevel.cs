using Aero.Domain.Interface;
using Aero.Domain.Interfaces;

namespace Aero.Infrastructure.Persistences.Entities
{
    public sealed class AccessLevel : BaseEntity
    {
        public string name { get;  set; } = string.Empty;
        public ICollection<AccessLevelComponent> components { get;  set;}
        public ICollection<CardHolderAccessLevel> cardholder_access_levels { get;  set;}

        public AccessLevel(string name,List<AccessLevelComponent> component,int location) : base(location)
        {
            this.name = name;
            this.components = component;
        }

        public void Update(Aero.Domain.Entities.AccessLevel data)
        {
            name = data.Name;
            components = data.Components.Select(x => new AccessLevelComponent(x.DriverId,x.Mac,x.DoorId,x.AcrId,x.TimezoneId)).ToList();
        }
        
    }

}
