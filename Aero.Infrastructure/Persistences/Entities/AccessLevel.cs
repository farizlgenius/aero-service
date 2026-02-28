using Aero.Domain.Interface;
using Aero.Domain.Interfaces;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Aero.Infrastructure.Persistences.Entities
{
    public sealed class AccessLevel : BaseEntity,IDbFunc<Aero.Domain.Entities.AccessLevel>
    {
        public string name { get;  set; } = string.Empty;
        public ICollection<AccessLevelComponent>? components { get;  set;} 
        public ICollection<UserAccessLevel>? cardholder_access_levels { get;  set;}

        public AccessLevel(){}

        public AccessLevel(string name,List<AccessLevelComponent> component,int location) : base(location)
        {
            this.name = name;
            this.components = component;
        }

        public AccessLevel(Aero.Domain.Entities.AccessLevel data)
        {
            name = data.Name;
            components = data.Components.Select(x => new AccessLevelComponent(x.DriverId, x.Mac, x.DoorId, x.AcrId, x.TimezoneId)).ToList();
        }

        public void Update(Aero.Domain.Entities.AccessLevel data)
        {
            name = data.Name;
            components = data.Components.Select(x => new AccessLevelComponent(x.DriverId,x.Mac,x.DoorId,x.AcrId,x.TimezoneId)).ToList();
            updated_date = DateTime.UtcNow;
        }

       
    }

}
