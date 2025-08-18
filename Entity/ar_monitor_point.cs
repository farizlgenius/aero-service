using System.ComponentModel.DataAnnotations;

namespace HIDAeroService.Entity
{
    public sealed class ar_monitor_point
    {
        [Key]
        public int id { get; set; }
        public string name { get; set; }
        public string scp_ip { get; set; }
        public short sio_number { get; set; }
        public short mp_number { get; set; }
        public short ip_number { get; set; }
        public short icvt_num { get; set; }
        public short lf_code { get; set; }
        public short delay_entry { get; set; }
        public short delay_exit { get; set; }
    }
}
