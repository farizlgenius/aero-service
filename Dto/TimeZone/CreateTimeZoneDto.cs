using AeroService.DTO.Interval;

namespace AeroService.DTO.TimeZone
{
    public sealed class CreateTimeZoneDto : NoMacBaseDto
    {
        public string Name { get; set; }
        public short Mode { get; set; }
        public string ActiveTime { get; set; } = string.Empty;
        public string DeactiveTime { get; set; } = string.Empty;
        public List<IntervalDto> Intervals { get; set; }
    }
}
