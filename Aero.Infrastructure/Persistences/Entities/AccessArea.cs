
using Aero.Domain.Interface;
using Aero.Domain.Interfaces;

namespace Aero.Infrastructure.Persistences.Entities
{
    public sealed class AccessArea : BaseEntity,IDriverId
    {
        public short driver_id { get; set; }
        public string name { get; set; } = string.Empty;    
        public short multi_occ { get; set; }
        public short access_control { get; set; }
        public short occ_control { get; set; }
        public short occ_set { get; set; }
        public short occ_max { get; set; }
        public short occ_up { get; set; }
        public short occ_down { get; set; }
        public short area_flag { get; set; }
        public ICollection<Door> door_in { get; set; }
        public ICollection<Door> door_out { get; set; }

        public AccessArea(short driver,string name,short multi_occ,short access_control,short occ_control,short occ_set,short occ_max,short occ_up,short occ_down,short area_flag,List<Door> door_in,List<Door> door_out,int location_id) : base(location_id) 
        {
            this.driver_id = driver_id;
            this.name = name;
            this.multi_occ = multi_occ;
            this.access_control = access_control;
            this.occ_control = occ_control;
            this.occ_set = occ_set;
            this.occ_max = occ_max;
            this.occ_up = occ_up;
            this.occ_down = occ_down;
            this.area_flag = area_flag;
            this.door_in = door_in;
            this.door_out = door_out;
        }

        public void Update(short driver, string name, short multi_occ, short access_control, short occ_control, short occ_set, short occ_max, short occ_up, short occ_down, short area_flag, List<Door> door_in, List<Door> door_out)
        {
            this.driver_id = driver_id;
            this.name = name;
            this.multi_occ = multi_occ;
            this.access_control = access_control;
            this.occ_control = occ_control;
            this.occ_set = occ_set;
            this.occ_max = occ_max;
            this.occ_up = occ_up;
            this.occ_down = occ_down;
            this.area_flag = area_flag;
            this.door_in = door_in;
            this.door_out = door_out;
        }

    }


}
