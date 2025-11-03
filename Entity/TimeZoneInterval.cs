namespace HIDAeroService.Entity
{
    public sealed class TimeZoneInterval : NoMacBaseEntity
    {
        public short TimeZoneId {  get; set; }
        public TimeZone TimeZone { get; set; }
        public short IntervalId { get; set; }
        public Interval Interval { get; set; }
    }
}
