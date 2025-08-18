namespace HIDAeroService.Dto.Time
{
    public sealed class CreateIntervalDto
    {
        public short TzNumber { get; set; }
        public short IntervalNumner { get; set; }
        public DaysInWeekDto Days { get; set; }
        public string IStart { get; set; }
        public string IEnd { get; set; }
    }
}
