using Aero.Domain.Helpers;
using System;


namespace Aero.Domain.Entities;

public sealed class Interval : BaseDomain
{
    public int Id { get; set; }
        public DaysInWeek Days { get; set; } = new DaysInWeek();
        public string DaysDetail { get; set; } = string.Empty;
        public string StartTime { get; set; } = string.Empty;   
        public string EndTime { get; set; } = string.Empty;

    public Interval(DaysInWeek days,string detail,string start,string end,int location,bool status) : base(location,status)
    {
        this.Days = days;
        SetDaysDetail(detail);
        SetStart(start);
        SetEnd(end);
    }

    private void SetDaysDetail(string detail)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(detail);
        if (!RegexHelper.IsValidName(detail.Trim())) throw new ArgumentException("Days detail invalid.");
        DaysDetail = detail;
    }

    private void SetStart(string start)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(start);
        var sanitized = start.Trim().Replace(":", string.Empty).Replace("-", string.Empty);
        if (!RegexHelper.IsValidOnlyCharAndDigit(sanitized)) throw new ArgumentException("Start time invalid.");
        this.StartTime = start;
    }

    private void SetEnd(string end) 
    { 
        ArgumentException.ThrowIfNullOrWhiteSpace(end); 
        var sanitized = end.Trim().Replace(":", string.Empty).Replace("-", string.Empty);
        if (!RegexHelper.IsValidOnlyCharAndDigit(sanitized)) throw new ArgumentException("End time invalid.");
        this.EndTime = end; 
    }
}
