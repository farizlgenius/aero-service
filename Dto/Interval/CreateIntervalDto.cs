using HIDAeroService.Entity.Interface;

namespace HIDAeroService.DTO.Interval
{
    public sealed class CreateIntervalDto : NoMacBaseDto,IComponentId
    {
        public short ComponentId { get; set; }
        public DaysInWeekDto Days { get; set; }
        public string DaysDesc { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
    }
}
