
using Aero.Domain.Interfaces;

namespace Aero.Infrastructure.Persistences.Entities
{
    public sealed class Module : BaseEntity,IMac,IDriverId
    {
        public short driver_id { get; set; }
        public short model { get; set; }
        public string model_desc { get; set; } = string.Empty;
        public string revision { get; set; } = string.Empty;
        public string serial_number { get; set; } = string.Empty;
        public int n_hardware_id { get; set; }
        public string n_hardware_id_desc { get; set; } = string.Empty;
        public int n_hardware_rev { get; set; }
        public int n_product_id { get; set; }
        public int n_product_ver { get; set; }
        public short n_enc_config { get; set; }
        public string n_enc_config_desc { get; set; } = string.Empty;
        public short n_enc_key_status { get; set; }
        public string n_enc_key_status_desc { get; set; } = string.Empty;
        public string mac { get; set; } = string.Empty;
        public Device device { get; set; }
        // HardwareComponent 
        public ICollection<Reader>? readers { get; set; }
        public ICollection<Sensor>? sensors { get; set; }
        public ICollection<Strike>? strikes { get; set; }
        public ICollection<RequestExit>? request_exits { get; set; }
        public ICollection<MonitorPoint>? monitor_points { get; set; }
        public ICollection<ControlPoint>? control_points { get; set; }
        // End
        public short address { get; set; }
        public string address_desc { get; set; } = string.Empty;
        public short port { get; set; }
        public short n_input { get; set; }
        public short n_output { get; set; }
        public short n_reader { get; set; }
        public short msp1_no { get; set; }
        public short baudrate { get; set; }
        public short n_protocol { get; set; }
        public short n_dialect { get; set; }
    }
}
