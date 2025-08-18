using HID.Aero.ScpdNet.Wrapper;
using System.ComponentModel.DataAnnotations;

namespace HIDAeroService.Entity
{
    public sealed class ar_tz 
    {
        [Key]
        public int id { get; set; }
        public string name { get; set; }
        public short tz_number { get; set; }
        public short mode { get; set; }
        public short act_time { get; set; }
        public short deact_time { get; set; }
        public short intervals { get; set; }


    }
}
