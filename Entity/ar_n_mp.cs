using System.ComponentModel.DataAnnotations;

namespace HIDAeroService.Entity
{
    public sealed class ar_n_mp
    {
        [Key]
        public int id { get; set; }
        public string scp_ip { get; set; }
        public short sio_number { get; set; }
        public short mp_number { get; set; }
        public short ip_number { get; set; }
        public bool is_available { get; set; }
    }
}
