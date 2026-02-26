
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
        public Device device { get; set; }

        public MonitorGroup(Aero.Domain.Entities.MonitorGroup data) : base(data.LocationId)
        {
            this.driver_id = data.DriverId;
            this.name = data.Name;
            this.n_mp_count = data.nMpCount;
            this.n_mp_list = data.nMpList.Select(x => new Aero.Infrastructure.Persistences.Entities.MonitorGroupList(x)).ToList();
            this.mac = data.Mac;
        }
    }
}
