using HIDAeroService.DTO.Interval;
using HIDAeroService.Entity;

namespace HIDAeroService.Aero.CommandService
{
    public interface ITimezoneCommandService
    {
        Task<bool> ExtendedTimeZoneActSpecificationAsync(short scpId, Entity.TimeZone dto, List<Interval> intervals, int activeTime, int deactiveTime);
        Task<bool> TimeZoneControlAsync(short ScpId, short Component, short Command);
        //string DaysInWeekToString(DaysInWeekDto days);
        //DaysInWeekDto StringToDaysInWeek(string daysString);
        //int ConvertDayToBinary(DaysInWeek days);
        //int ConvertTimeToEndMinute(string timeString);
    }
}
