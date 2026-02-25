
using System.ComponentModel.DataAnnotations;
using Aero.Domain.Interface;


namespace Aero.Infrastructure.Persistences.Entities
{
    public sealed class DaysInWeek : BaseEntity,IDatetime
    {
        public int interval_id { get; set; }
        public Interval interval { get; set; }
        public bool sunday { get; set; }
        public bool monday { get; set; }
        public bool tuesday { get; set; }
        public bool wednesday { get; set; }
        public bool thursday { get; set; }
        public bool friday { get; set; }
        public bool saturday { get; set; }

        public DaysInWeek(int interval_id,bool sun,bool mon,bool tue,bool wed,bool thur,bool fri,bool sat,int locaion) : base(locaion)
        {
            this.interval_id = interval_id;
            this.sunday = sun;
            this.monday = mon;
            this.tuesday = tue;
            this.wednesday = wed;
            this.thursday = thur;
            this.friday = fri;
            this.saturday = sat;

        }
    }
}
