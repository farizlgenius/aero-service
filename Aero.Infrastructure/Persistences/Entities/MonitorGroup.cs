
using Aero.Domain.Interfaces;

namespace Aero.Infrastructure.Persistences.Entities
{
    public sealed class MonitorGroup : BaseEntity,IMac,IDriverId
    {
        public short driver_id { get; set; }
        public string name { get; set; } = string.Empty;
        public short n_mp_count { get; set; }
        public ICollection<MonitorGroupList> n_mp_list { get; set; }
        public string mac { get; set; } = string.Empty;
        public Hardware hardware { get; set; }
    }
}
