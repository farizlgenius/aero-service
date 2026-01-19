
using Aero.Infrastructure.Data.Interface;

namespace Aero.Infrastructure.Data.Entities
{
    public sealed class Area : NoMacBaseEntity,IComponentId
    {
        public short component_id { get; set; }
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

    }
}
