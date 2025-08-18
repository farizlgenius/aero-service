using System.ComponentModel.DataAnnotations;

namespace HIDAeroService.Entity
{
    public class ar_n_acr
    {
        [Key]
        public int id { get; set; }
        public string scp_ip { get; set; }
        public short sio_number { get; set; }
        public short acr_number { get; set; }
        public bool is_available { get; set; }
    }
}
