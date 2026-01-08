using HIDAeroService.Entity.Interface;

namespace HIDAeroService.DTO.Interval
{
    public sealed class CreateIntervalDto : NoMacBaseDto,IComponentId
    {
        public short component_id { get; set; }
        public DaysInWeekDto Days { get; set; }
        public string DaysDesc { get; set; } = string.Empty;
        public string StartTime { get; set; } = string.Empty;
        public string EndTime { get; set; } = string.Empty;
    }
}
