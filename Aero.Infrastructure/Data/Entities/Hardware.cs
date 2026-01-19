
using System.ComponentModel.DataAnnotations;

namespace Aero.Infrastructure.Data.Entities
{
    public sealed class Hardware : BaseEntity
    {
        public string name { get; set; } = string.Empty;
        public int hardware_type { get; set; } 
        public string hardware_type_desc { get; set; } = string.Empty;
        public ICollection<Module> modules { get; set; }
        public ICollection<HardwareCredential> hardware_credentials { get; set; }
        public ICollection<HardwareAccessLevel> hardware_accesslevels { get; set; }
        public ICollection<Door> doors { get; set; }
        public ICollection<MonitorGroup> monitor_groups { get; set; }
        public ICollection<Procedure> procedures { get; set; }
        public string ip { get; set; } = string.Empty;
        public string mac { get; set; } = string.Empty;
        public string port { get; set; } = string.Empty;
        public string firmware { get; set; } = string.Empty;
        public string serial_number { get; set; } = string.Empty;
        public bool port_one { get; set; } = false;
        public short protocol_one { get; set; }
        public string protocol_one_desc { get; set; }= string.Empty;
        public short baudrate_one { get; set; }
        public bool port_two { get; set; } = false;
        public short protocol_two { get; set; }
        public string protocol_two_desc { get; set; } = string.Empty;
        public short baudrate_two { get; set; }
        public bool is_upload { get; set; } = false;
        public bool is_reset { get; set; } = false;
        public DateTime last_sync { get; set; }
    }
}
