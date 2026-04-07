using System.ComponentModel.DataAnnotations;
using Aero.Domain.Interface;
using Aero.Domain.Interfaces;

namespace Aero.Infrastructure.Persistences.Entities
{
    public sealed class Procedure : BaseEntity,IDeviceId
    {
        public int scp_id { get; set; }
        public Device device { get; set; }

        public short procedure_id { get; set; }
        public string name { get; set; } = string.Empty;
        public int trigger_id {get; set;}
        public Trigger trigger { get; set; }
        public ICollection<Action> actions { get; set; }

        public Procedure(){}


        public Procedure(Aero.Domain.Entities.Procedure data) : base(data.LocationId)
        {
            this.scp_id = data.ScpId;
            this.procedure_id = data.ProcedureId;
            this.name = data.Name;
            this.trigger_id = data.TriggerId;
        }

        public void Update(Aero.Domain.Entities.Procedure data) 
        {
            this.name = data.Name;
            this.trigger_id = data.TriggerId;
            this.updated_date = DateTime.UtcNow;
        }



    }
}

