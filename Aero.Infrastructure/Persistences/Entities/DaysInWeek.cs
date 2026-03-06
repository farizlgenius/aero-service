
using Aero.Domain.Interface;
using System.ComponentModel.DataAnnotations;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace Aero.Infrastructure.Persistences.Entities
{
    public sealed class DaysInWeek 
    {
        [Key]
        public int id {get; set;}        
        public int interval_id { get; set; }
        public Interval interval { get; set; }
        public bool sunday { get; set; }
        public bool monday { get; set; }
        public bool tuesday { get; set; }
        public bool wednesday { get; set; }
        public bool thursday { get; set; }
        public bool friday { get; set; }
        public bool saturday { get; set; }

        public DaysInWeek(){}


        public DaysInWeek(int interval_id,bool sun,bool mon,bool tue,bool wed,bool thur,bool fri,bool sat) 
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

        public DaysInWeek(Aero.Domain.Entities.DaysInWeek data) 
        {
            this.interval_id = data.IntervalId;
            this.sunday = data.Sunday;
            this.monday = data.Monday;
            this.tuesday = data.Tuesday;
            this.wednesday = data.Wednesday;
            this.thursday = data.Thursday;
            this.friday = data.Friday;
            this.saturday = data.Saturday;
        }

        public void Update(Aero.Domain.Entities.DaysInWeek data)
        {
            this.interval_id = data.IntervalId;
            this.sunday = data.Sunday;
            this.monday = data.Monday;
            this.tuesday = data.Tuesday;
            this.wednesday = data.Wednesday;
            this.thursday = data.Thursday;
            this.friday = data.Friday;
            this.saturday = data.Saturday;
        }
    }
}

