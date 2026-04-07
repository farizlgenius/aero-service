
using Aero.Domain.Entities;
using Aero.Domain.Interfaces;

namespace Aero.Infrastructure.Persistences.Entities
{
    public sealed class Sensor : BaseEntity,IDeviceId
    {
        public Module module { get; set; }
        public int door_id { get; set; }
        public Door sensor_door { get; set; }
        public int module_id { get; set; }
        public short input_no { get; set; }
        public short input_mode { get; set; }
        public short debounce { get; set; }
        public short holdtime { get; set; }
        public short dc_held { get; set; }
        public int scp_id { get; set; }
        public Device device { get; set; }

        public Sensor(){}


        public Sensor(int scp_id,int module,int doorid,short inputno,short inputmode,short debounce,short holdtime,short dcheld,int location) : base(location) 
        {
            this.scp_id = scp_id;
            this.module_id = module;
            this.door_id = doorid;
            this.input_no = inputno;
            this.input_mode = inputmode;
            this.debounce = debounce;
            this.holdtime = holdtime;
            this.dc_held = dcheld;

        }

        public Sensor(Aero.Domain.Entities.Sensor data)
        {
            this.scp_id = data.ScpId;
            this.module_id = data.ModuleId;
            this.door_id = data.DoorId;
            this.input_no = data.InputNo;
            this.input_mode = data.InputMode;
            this.debounce = data.Debounce;
            this.holdtime = data.HoldTime;
            this.dc_held = data.DcHeld;

        }

        public void Update(Aero.Domain.Entities.Sensor data)
        {
            this.scp_id = data.ScpId;
            this.module_id = data.ModuleId;
            this.door_id = data.DoorId;
            this.input_no = data.InputNo;
            this.input_mode = data.InputMode;
            this.debounce = data.Debounce;
            this.holdtime = data.HoldTime;
            this.dc_held = data.DcHeld;
            this.updated_date = DateTime.UtcNow;
        }

    }
}

