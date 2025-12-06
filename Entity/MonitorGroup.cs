using HIDAeroService.Model;

namespace HIDAeroService.Entity
{
    public sealed class MonitorGroup : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public short nMpCount { get; set; }
        public ICollection<MonitorGroupList> nMpList { get; set; }
    }
}
