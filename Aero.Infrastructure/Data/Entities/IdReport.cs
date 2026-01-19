using System.ComponentModel.DataAnnotations;

namespace Aero.Infrastructure.Data.Entities
{
    public sealed class IdReport
    {
        [Key]
        public int id { get; set; }
        public short device_id { get; set; }
        public short device_ver { get; set; }
        public short software_rev_major { get; set; }
        public short software_rev_minor { get; set; }
        public string firmware { get; set; } = string.Empty;
        public int serial_number { get; set; }
        public int ram_size { get; set; }
        public int ram_free { get; set; }
        public DateTimeOffset e_sec { get; set; }
        public int db_max { get; set; }
        public int db_active { get; set; }
        public byte dip_switch_powerup { get; set; }
        public byte dip_switch_current { get; set; }
        public short scp_id { get; set; }
        public short firmware_advisory { get; set; }
        public short scp_in1 { get; set; }
        public short scp_in2 { get; set; }
        public short n_oem_code { get; set; }
        public byte config_flag { get; set; }
        public string mac { get; set; } = string.Empty;
        public byte tls_status { get; set; }
        public byte oper_mode { get; set; }
        public short scp_in3 { get; set; }
        public int cumulative_bld_cnt { get; set; }
        public string ip { get; set; } = string.Empty;
        public string port { get; set; } = string.Empty;

    }
}
