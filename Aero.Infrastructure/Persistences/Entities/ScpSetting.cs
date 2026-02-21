using System.ComponentModel.DataAnnotations;

namespace Aero.Infrastructure.Persistences.Entities
{
    public sealed class ScpSetting
    {
        [Key]
        public int id { get; set; }
        public short n_msp1_port { get; set; }
        public int n_transaction { get; set; }
        public short n_sio { get; set; }
        public short n_mp { get; set; }
        public short n_cp { get; set; }
        public short n_acr { get; set; }
        public short n_alvl { get; set; }
        public short n_trgr { get; set; }
        public short n_proc { get; set; }
        public short gmt_offset { get; set; }
        public short n_tz { get; set; }
        public short n_hol { get; set; }
        public short n_mpg { get; set; }
        public short n_card { get; set; }
        public short n_area { get; set; }
    }
}
