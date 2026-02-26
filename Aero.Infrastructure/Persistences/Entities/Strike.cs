
using Aero.Domain.Entities;

namespace Aero.Infrastructure.Persistences.Entities
{
    public sealed class Strike : BaseEntity
    {
        public int module_id { get; set; }
        public Module module { get; set; }
        public Door strike_door { get; set; }
        public int door_id { get; set; }
        public short output_no { get; set; }
        public short relay_mode { get; set; }
        public short offline_mode { get; set; }
        public short strike_max { get; set; }
        public short strike_min { get; set; }
        public short strike_mode { get; set; }

        public Strike(int moduleid,int doorid,short outputno,short relaymode,short offlinemode,short strikmax,short strikemin,short strikemode,int location) : base(location) 
        {
            this.module_id = moduleid;
            this.door_id = doorid;
            this.output_no = outputno;
            this.relay_mode = relaymode;
            this.offline_mode = offlinemode;
            this.strike_max = strikmax;
            this.strike_min = strikemin;
            this.strike_mode = strikemode;
        }

        public void Update(Aero.Domain.Entities.Strike data)
        {
            this.module_id = data.ModuleId;
            this.door_id = data.DoorId;
            this.output_no = data.OutputNo;
            this.relay_mode = data.RelayMode;
            this.offline_mode = data.OfflineMode;
            this.strike_max = data.StrkMax;
            this.strike_min = data.StrkMin;
            this.strike_mode = data.StrkMode;
        }

    }
}
