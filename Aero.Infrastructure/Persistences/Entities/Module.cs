
using Aero.Domain.Interfaces;

namespace Aero.Infrastructure.Persistences.Entities
{
    public sealed class Module : BaseEntity,IMac,IDriverId
    {
        public short driver_id { get; set; }
        public short model { get; set; }
        public string model_detail { get; set; } = string.Empty;
        public string revision { get; set; } = string.Empty;
        public string serial_number { get; set; } = string.Empty;
        public int n_hardware_id { get; set; }
        public string n_hardware_id_detail { get; set; } = string.Empty;
        public int n_hardware_rev { get; set; }
        public int n_product_id { get; set; }
        public int n_product_ver { get; set; }
        public short n_enc_config { get; set; }
        public string n_enc_config_detail { get; set; } = string.Empty;
        public short n_enc_key_status { get; set; }
        public string n_enc_key_status_detail { get; set; } = string.Empty;
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
        public string address_detail { get; set; } = string.Empty;
        public short port { get; set; }
        public short n_input { get; set; }
        public short n_output { get; set; }
        public short n_reader { get; set; }
        public short msp1_no { get; set; }
        public short baudrate { get; set; }
        public short n_protocol { get; set; }
        public short n_dialect { get; set; }
        public Module(Aero.Domain.Entities.Module data) : base(data.LocationId)
        {
            this.driver_id = data.DriverId;
            this.model = data.Model;
            this.model_detail = data.ModelDetail;
            this.revision = data.Revision;
            this.serial_number = data.SerialNumber;
            this.n_hardware_id = data.nHardwareId;
            this.n_hardware_id_detail = data.nHardwareIdDetail;
            this.n_hardware_rev = data.nHardwareRev;
            this.n_product_id = data.nProductId;
            this.n_product_ver = data.nProductVer;
            this.n_enc_config = data.nEncConfig;
            this.n_enc_config_detail = data.nEncConfigDetail;
            this.n_enc_key_status = data.nEncKeyStatus;
            this.n_enc_key_status_detail = data.nEncKeyStatusDetail;
            this.mac = this.mac;
            this.address = data.Address;
            this.address_detail = data.AddressDetail;
            this.port = data.Port;
            this.n_input = data.nInput;
            this.n_output = data.nOutput;
            this.n_reader = data.nReader;
            this.msp1_no = data.Msp1No;
            this.baudrate = data.BaudRate;
            this.n_protocol = data.nProtocol;
            this.n_dialect = data.nDialect;
        }

        public void Update(Aero.Domain.Entities.Module data) 
        {
            this.model = data.Model;
            this.model_detail = data.ModelDetail;
            this.revision = data.Revision;
            this.serial_number = data.SerialNumber;
            this.n_hardware_id = data.nHardwareId;
            this.n_hardware_id_detail = data.nHardwareIdDetail;
            this.n_hardware_rev = data.nHardwareRev;
            this.n_product_id = data.nProductId;
            this.n_product_ver = data.nProductVer;
            this.n_enc_config = data.nEncConfig;
            this.n_enc_config_detail = data.nEncConfigDetail;
            this.n_enc_key_status = data.nEncKeyStatus;
            this.n_enc_key_status_detail = data.nEncKeyStatusDetail;
            this.address = data.Address;
            this.address_detail = data.AddressDetail;
            this.port = data.Port;
            this.n_input = data.nInput;
            this.n_output = data.nOutput;
            this.n_reader = data.nReader;
            this.msp1_no = data.Msp1No;
            this.baudrate = data.BaudRate;
            this.n_protocol = data.nProtocol;
            this.n_dialect = data.nDialect;
        }
    }
}
