
using Aero.Domain.Interface;

namespace Aero.Infrastructure.Persistences.Entities
{
    public sealed class Holiday : BaseEntity
    {
        public short year { get; set; }
        public short month { get; set; }
        public short day { get; set; }
        public short extend { get; set; }
        public short type_mask { get; set; }

        public Holiday(short year,short month,short day,short extend,short mask,int location) : base(location)
        {
            this.year = year;
            this.month = month;
            this.day = day;
            this.extend = extend;
            this.type_mask = mask;
        }

        public void Update(Aero.Domain.Entities.Holiday data) 
        {
            this.year = data.Year;
            this.month = data.Month;
            this.day = data.Day;
            this.extend = data.Extend;
            this.type_mask = data.TypeMask;
        }
    }
}
