
using Aero.Domain.Entities;

namespace Aero.Application.DTOs
{
    public sealed class MonitorGroupDto : BaseDomain
    {
        public string Name { get; set; } = string.Empty;
        public short nMpCount { get; set; }
        public List<MonitorGroupListDto> nMpList { get; set; } = new List<MonitorGroupListDto>();
    }
}
