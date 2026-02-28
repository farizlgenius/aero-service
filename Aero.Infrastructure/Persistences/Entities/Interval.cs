


using Aero.Domain.Interface;

namespace Aero.Infrastructure.Persistences.Entities
{
    public sealed class Interval : BaseEntity
    {
        public DaysInWeek days { get; set; }
        public ICollection<TimeZoneInterval> timezone_intervals { get; set; }
        public string days_detail { get; set; } = string.Empty;
        public string start_time { get; set; } = string.Empty;
        public string end_time { get; set; } = string.Empty;

        public Interval(Aero.Domain.Entities.DaysInWeek days,string daydetail,string start,string end,int location) : base(location)
        {
            this.days = new DaysInWeek(days.IntervalId,days.Sunday,days.Monday,days.Tuesday,days.Wednesday,days.Thursday,days.Friday,days.Saturday,location);
            this.days_detail = daydetail;
            this.start_time = start;
            this.end_time = end;

        }

        public Interval(Aero.Domain.Entities.Interval data)
        {
            this.days = new DaysInWeek(data.Id, data.Days.Sunday, data.Days.Monday, data.Days.Tuesday, data.Days.Wednesday, data.Days.Thursday, data.Days.Friday, data.Days.Saturday, data.LocationId);
            this.days_detail = data.DaysDetail;
            this.start_time = data.StartTime;
            this.end_time = data.EndTime;

        }

        public void Update(Aero.Domain.Entities.Interval data)
        {
            this.days = new DaysInWeek(data.Id, data.Days.Sunday, data.Days.Monday, data.Days.Tuesday, data.Days.Wednesday, data.Days.Thursday, data.Days.Friday, data.Days.Saturday, data.LocationId);
            this.days_detail = data.DaysDetail;
            this.start_time = data.StartTime;
            this.end_time = data.EndTime;
            this.updated_date = DateTime.UtcNow;

        }



    }
}
