using AeroService.DTO.Interval;
using AeroService.Entity;

namespace AeroService.Aero.CommandService
{
    public interface ITimeZoneCommandService 
    {
        Task<bool> ExtendedTimeZoneActSpecificationAsync(short scpId, Entity.TimeZone dto, List<Interval> intervals, int activeTime, int deactiveTime);
        Task<bool> TimeZoneControlAsync(short ScpId, short Component, short Command);
        //string DaysInWeekToString(DaysInWeekDto days);
        //DaysInWeekDto StringToDaysInWeek(string daysString);
        //int ConvertDayToBinary(DaysInWeek days);
        //int ConvertTimeToEndMinute(string timeString);
    }
}
