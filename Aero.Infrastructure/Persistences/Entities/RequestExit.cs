using Aero.Domain.Entities;

namespace Aero.Infrastructure.Persistences.Entities
{
    public class RequestExit : BaseEntity
    {
        public Module module { get; set; }
        public short door_id { get; set; }
        public Door door { get; set; }
        public short module_id { get; set; }
        public short input_no { get; set; }
        public short input_mode { get; set; }
        public short debounce { get; set; }
        public short holdtime { get; set; }
        public short mask_timezone { get; set; } = 0;
        public RequestExit(short module,short doorid,short input,short mode,short debounce,short holdtime,short mask,int location) : base(location)
        {
            this.module_id = module;
            this.door_id = doorid;
            this.input_no = input;
            this.input_mode = mode;
            this.debounce = debounce;
            this.holdtime = holdtime;
            this.mask_timezone = mask;
        }

        public void Update(Aero.Domain.Entities.RequestExit data)
        {
            this.module_id = data.InputMode;
            this.door_id = data.DoorId;
            this.input_no = data.InputNo;
            this.input_mode = data.InputMode;
            this.debounce = data.Debounce;
            this.holdtime = data.HoldTime;
            this.mask_timezone = data.MaskTimeZone;
        }
    }
}
