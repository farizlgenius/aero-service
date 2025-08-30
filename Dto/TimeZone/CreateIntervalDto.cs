namespace HIDAeroService.Dto.TimeZone
{
    public sealed class CreateIntervalDto
    {
        public short ComponentNo { get; set; }
        public DaysInWeekDto Days { get; set; }
        public string IStart { get; set; }
        public string IEnd { get; set; }
    }
}
