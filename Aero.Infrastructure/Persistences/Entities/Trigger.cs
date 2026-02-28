using Aero.Domain.Entities;
using Aero.Domain.Interfaces;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Aero.Infrastructure.Persistences.Entities
{
    public sealed class Trigger : BaseEntity, IDriverId, IDeviceId
    {
        public short driver_id { get; set; }
        public string name { get; set; } = string.Empty;
        public short command { get; set; }
        public short procedure_id { get; set; }
        public short source_type { get; set; }
        public short source_number { get; set; }
        public short tran_type { get; set; }
        public int device_id { get; set; } 
        public Device device { get; set; }
        public ICollection<TriggerTranCode> code_map { get; set; }
        public short timezone { get; set; }
        public Procedure procedure { get; set; }

        public Trigger(Aero.Domain.Entities.Trigger data) : base(data.LocationId)
        {
            this.driver_id = data.DriverId;
            this.name = data.Name;
            this.command = data.Command;
            this.procedure_id = data.ProcedureId;
            this.source_type = data.SourceType;
            this.source_number = data.SourceNumber;
            this.tran_type = data.TranType;
            this.device_id = data.DeviceId;
            this.code_map = data.CodeMap.Select(x => new TriggerTranCode(x)).ToList();
            this.timezone = data.TimeZone;

        }

        public void Update(Aero.Domain.Entities.Trigger data)
        {

            this.name = data.Name;
            this.command = data.Command;
            this.procedure_id = data.ProcedureId;
            this.source_type = data.SourceType;
            this.source_number = data.SourceNumber;
            this.tran_type = data.TranType;
            this.device_id = data.DeviceId;
            this.code_map = data.CodeMap.Select(x => new TriggerTranCode(x)).ToList();
            this.timezone = data.TimeZone;
        }

    }
}
