using Aero.Domain.Entities;
using Aero.Domain.Interfaces;

namespace Aero.Infrastructure.Persistences.Entities
{
    public class RequestExit : BaseEntity,IDeviceId
    {
        public Module module { get; set; }
        public int door_id { get; set; }
        public Door door { get; set; }
        public int module_id { get; set; }
        public short input_no { get; set; }
        public short input_mode { get; set; }
        public short debounce { get; set; }
        public short holdtime { get; set; }
        public short mask_timezone { get; set; } = 0;
        public int scp_id { get; set; }
        public Device device { get; set; }
        public RequestExit(){}

        public RequestExit(int scp_id,int module_id,int door_id,short input_no,short input_mode,short debounce,short holdtime,short mask_timezone,int location) : base(location)
        {
            this.scp_id = scp_id;
            this.module_id = module_id;
            this.door_id = door_id;
            this.input_no = input_no;
            this.input_mode = input_mode;
            this.debounce = debounce;
            this.holdtime = holdtime;
            this.mask_timezone = mask_timezone;
        }

        public RequestExit(Aero.Domain.Entities.RequestExit data)
        {
            this.scp_id = data.ScpId;
            this.module_id = data.InputMode;
            this.door_id = data.DoorId;
            this.input_no = data.InputNo;
            this.input_mode = data.InputMode;
            this.debounce = data.Debounce;
            this.holdtime = data.HoldTime;
            this.mask_timezone = data.MaskTimeZone;
        }

        public void Update(Aero.Domain.Entities.RequestExit data)
        {
            this.scp_id = data.ScpId;
            this.module_id = data.InputMode;
            this.door_id = data.DoorId;
            this.input_no = data.InputNo;
            this.input_mode = data.InputMode;
            this.debounce = data.Debounce;
            this.holdtime = data.HoldTime;
            this.mask_timezone = data.MaskTimeZone;
            this.updated_date = DateTime.UtcNow;
        }
    }
}

