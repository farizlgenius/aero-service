namespace HIDAeroService.Dto.TimeZone
{
    public sealed class TimeZoneDto
    {
        public string Name { get; set; }
        public short ComponentNo { get; set; }
        public short Mode { get; set; }
        public string ActiveTime { get; set; } = string.Empty;
        public string DeactiveTime { get; set; } = string.Empty;
        public short Intervals { get; set; }
        public string IntervalsNoList { get; set; } = string.Empty;
        public bool IsActive { get; set; }

    }
}
