

namespace Aero.Application.DTOs
{
    public sealed class TimeZoneDto : NoMacBaseDto
    {
        public short ComponentId { get; set; }
        public string Name { get; set; } = string.Empty;
        public short Mode { get; set; }
        public string ActiveTime { get; set; } = string.Empty;
        public string DeactiveTime { get; set; } = string.Empty;
        public List<IntervalDto>? Intervals { get; set; }

    }
}
