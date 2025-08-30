namespace HIDAeroService.Dto.TimeZone
{
    public sealed class CreateTimeZoneDto
    {
        public string Name { get; set; }
        public string ActiveTime { get; set; }
        public string DeactiveTime { get; set; }
        public short Intervals { get; set; }
        public CreateIntervalDto[] IntervalsDetail { get; set; }

    }
}
