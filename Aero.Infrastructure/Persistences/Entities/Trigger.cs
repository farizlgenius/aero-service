using Aero.Domain.Entities;
using Aero.Domain.Interfaces;

namespace Aero.Infrastructure.Persistences.Entities
{
    public sealed class Trigger : BaseEntity,IDriverId,IMac
    {
        public short driver_id {get; set;}
        public string name { get; set; } = string.Empty;
        public short command { get; set; }
        public short procedure_id { get; set; }
        public short source_type { get; set; }
        public short source_number { get; set; }
        public short tran_type { get; set; }
        public string mac { get; set; } = string.Empty;
        public Device hardware { get; set; }
        public ICollection<TriggerTranCode> code_map { get; set; }
        public short timezone { get; set; }
        public Procedure procedure { get; set; }

    }
}
