using System.ComponentModel.DataAnnotations;

namespace HIDAeroService.Entity
{
    public sealed class SystemConfiguration 
    {
        [Key]
        public int id { get; set; }
        public short n_ports { get; set; }
        public short n_scp { get; set; }
        public short n_channel_id { get; set; }
        public short c_type { get; set; }
        public short c_port { get; set; }

    }
}
