using System;


namespace Aero.Domain.Entities;

public sealed class Interval : BaseDomain
{
    public int Id { get; set; }
        public DaysInWeek? Days { get; set; }
        public string DaysDetail { get; set; } = string.Empty;
        public string StartTime { get; set; } = string.Empty;   
        public string EndTime { get; set; } = string.Empty;

    public Interval(int id,DaysInWeek days,string detail,string start,string end,int location,bool status) : base(location,status)
    {
        this.Id = id;
        this.Days = days;
        this.DaysDetail = detail;
        this.StartTime = start;
        this.EndTime = end;
    }
}
