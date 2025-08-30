using HIDAeroService.Dto.TimeZone;

namespace HIDAeroService.Dto.Interval
{
    public sealed class IntervalDto
    {
        public short ComponentNo { get; set; }
        public DaysInWeekDto Days { get; set; }
        public string StartTime { get; set; }
        public string Endtime { get; set; }
    }
}
