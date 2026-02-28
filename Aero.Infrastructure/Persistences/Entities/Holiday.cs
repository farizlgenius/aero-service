
using Aero.Domain.Interface;
using Aero.Domain.Interfaces;

namespace Aero.Infrastructure.Persistences.Entities
{
    public sealed class Holiday : BaseEntity,IDriverId
    {
        public string name { get; set; } = string.Empty;
        public short driver_id { get; set; }
        public short year { get; set; }
        public short month { get; set; }
        public short day { get; set; }
        public short extend { get; set; }
        public short type_mask { get; set; }

        public Holiday(short driver,string name,short year,short month,short day,short extend,short mask,int location) : base(location)
        {
            this.name = name;
            this.driver_id = driver;
            this.year = year;
            this.month = month;
            this.day = day;
            this.extend = extend;
            this.type_mask = mask;
        }

        public void Update(Aero.Domain.Entities.Holiday data) 
        {
            this.name = data.Name;
            this.year = data.Year;
            this.month = data.Month;
            this.day = data.Day;
            this.extend = data.Extend;
            this.type_mask = data.TypeMask;
            this.updated_date = DateTime.UtcNow;
        }
    }
}
