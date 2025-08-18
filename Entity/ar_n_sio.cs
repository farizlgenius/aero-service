using System.ComponentModel.DataAnnotations;

namespace HIDAeroService.Entity
{
    public sealed class ar_n_sio
    {
        [Key]
        public int id { get; set; }
        public string scp_ip { get; set; }
        public short sio_number { get; set; }
        public bool is_available { get; set; }
        public short port { get; set; }
    }
}
