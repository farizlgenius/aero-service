using Aero.Domain.Entities;


namespace Aero.Application.DTOs
{
    public sealed class IntervalDto : NoMacBaseEntity
    {
        public DaysInWeekDto Days { get; set; }
        public string DaysDesc { get; set; } = string.Empty;
        public string StartTime { get; set; } = string.Empty;   
        public string EndTime { get; set; } = string.Empty;
    }
}
