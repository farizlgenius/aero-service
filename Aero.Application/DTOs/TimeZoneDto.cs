

using Aero.Domain.Entities;

namespace Aero.Application.DTOs
{
    public sealed class TimeZoneDto : NoMacBaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public short Mode { get; set; }
        public string ActiveTime { get; set; } = string.Empty;
        public string DeactiveTime { get; set; } = string.Empty;
        public List<IntervalDto> Intervals { get; set; } = new List<IntervalDto>();

    }
}
