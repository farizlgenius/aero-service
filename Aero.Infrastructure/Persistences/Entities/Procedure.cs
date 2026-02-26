using System.ComponentModel.DataAnnotations;
using Aero.Domain.Interface;
using Aero.Domain.Interfaces;

namespace Aero.Infrastructure.Persistences.Entities
{
    public sealed class Procedure : BaseEntity,IDriverId
    {

        public short driver_id { get; set; }
        public string name { get; set; } = string.Empty;
        public int trigger_id {get; set;}
        public Trigger trigger { get; set; }
        public ICollection<Action> actions { get; set; }

        public Procedure(Aero.Domain.Entities.Procedure data) : base(data.LocationId)
        {
            this.driver_id = data.DriverId;
            this.name = data.Name;
            this.trigger_id = data.TriggerId;
        }



    }
}
