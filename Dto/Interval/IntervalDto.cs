namespace HIDAeroService.DTO.Interval
{
    public sealed class IntervalDto : NoMacBaseDto
    {
        public short ComponentId { get; set; }
        public DaysInWeekDto Days { get; set; }
        public string DaysDesc { get; set; } = string.Empty;
        public string StartTime { get; set; } = string.Empty;   
        public string EndTime { get; set; } = string.Empty;
    }
}
