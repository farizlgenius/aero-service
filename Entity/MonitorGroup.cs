using AeroService.Entity.Interface;
using AeroService.Model;

namespace AeroService.Entity
{
    public sealed class MonitorGroup : BaseEntity
    {
        public string name { get; set; } = string.Empty;
        public short n_mp_count { get; set; }
        public ICollection<MonitorGroupList> n_mp_list { get; set; }
        public string hardware_mac { get; set; } = string.Empty;
        public Hardware hardware { get; set; }
    }
}
