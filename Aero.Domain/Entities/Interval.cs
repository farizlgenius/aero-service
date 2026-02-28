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
        this.DaysDetail = detail;
        SetStart(start);
        SetEnd(end);
    }

    private void SetStart(string start)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(start);
        this.StartTime = start;
    }

    private void SetEnd(string end) 
    { 
        ArgumentException.ThrowIfNullOrWhiteSpace(end); 
        this.EndTime = end; 
    }
}
