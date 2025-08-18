using System.ComponentModel.DataAnnotations;

namespace HIDAeroService.Entity
{
    public sealed class ar_scp 
    {
        [Key]
        public int id { get; set; }
        public short scp_id { get; set; }
        public string name { get; set; }
        public string model { get; set; }
        public string mac { get; set; }
        public string ip_address { get; set; }
        public string serial_number { get; set; }
        public short n_sio { get; set; }
        public short n_mp { get; set; }
        public short n_cp { get; set; }
        public short n_acr { get; set; }
        public short n_alvl { get; set; }
        public short n_trgr { get; set; }
        public short n_proc { get; set; }
        public short n_tz { get; set; }
        public short n_hol { get; set; }
        public short n_mpg { get; set; }

    }
}
