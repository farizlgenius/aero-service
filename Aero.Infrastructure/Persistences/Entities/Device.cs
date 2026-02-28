
using Aero.Application.Services;
using Aero.Domain.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Aero.Infrastructure.Persistences.Entities
{
    public sealed class Device : BaseEntity,IDriverId
    {
        public string name { get; set; } = string.Empty;
        public short driver_id { get; set; }
        public int hardware_type { get; set; } 
        public string hardware_type_detail { get; set; } = string.Empty;
        public ICollection<Module> modules { get; set; }
        public ICollection<DeviceCredential> device_credentials { get; set; }
        public ICollection<Door> doors { get; set; }
        public ICollection<MonitorGroup> monitor_groups { get; set; }
        public ICollection<MonitorPoint> monitor_points { get; set; }
        public ICollection<ControlPoint> control_points { get; set; }
        public ICollection<Procedure> procedures { get; set; }
        public ICollection<AccessLevelComponent> access_level_component {get; set;}
        public ICollection<Action> actions { get; set; }
        public ICollection<Reader> readers { get; set; }
        public ICollection<Sensor> sensors { get; set; }
        public ICollection<RequestExit> request_exits { get; set; }
        public ICollection<Strike> strikes { get; set; }
        public ICollection<UserDevice> user_device { get; set; }
        public string mac { get; set; } = string.Empty;
        public string ip { get; set; } = string.Empty;
        public string port { get; set; } = string.Empty;
        public string firmware { get; set; } = string.Empty;
        public string serial_number { get; set; } = string.Empty;
        public bool port_one { get; set; } = false;
        public short protocol_one { get; set; }
        public string protocol_one_detail { get; set; }= string.Empty;
        public short baudrate_one { get; set; }
        public bool port_two { get; set; } = false;
        public short protocol_two { get; set; }
        public string protocol_two_detail { get; set; } = string.Empty;
        public short baudrate_two { get; set; }
        public bool is_upload { get; set; } = false;
        public bool is_reset { get; set; } = false;
        public DateTime last_sync { get; set; }

        public Device(Aero.Domain.Entities.Device data) : base(data.LocationId)
        {
            this.name = data.Name;
            this.driver_id = data.DriverId;
            this.hardware_type = data.HardwareType;
            this.hardware_type_detail = data.HardwareTypeDetail;
            this.mac = data.Mac;
            this.ip = data.Ip;
            this.port = data.Port;
            this.firmware = data.Firmware;
            this.serial_number = data.SerialNumber;
            this.port_one = data.PortOne;
            this.protocol_one = data.ProtocolOne;
            this.protocol_one_detail = data.ProtocolOneDetail;
            this.baudrate_one = data.BaudRateOne;
            this.protocol_two_detail =  data.ProtocolTwoDetail;
            this.protocol_two = data.ProtocolTwo;
            this.baudrate_two = data.BaudRateTwo;
            this.is_upload = data.IsUpload;
            this.is_reset = data.IsReset;
            this.last_sync = data.LastSync;
        }

        public void Update(Aero.Domain.Entities.Device data) 
        {
            this.name = data.Name;
            this.hardware_type = data.HardwareType;
            this.hardware_type_detail = data.HardwareTypeDetail;
            this.mac = data.Mac;
            this.ip = data.Ip;
            this.port = data.Port;
            this.firmware = data.Firmware;
            this.serial_number = data.SerialNumber;
            this.port_one = data.PortOne;
            this.protocol_one = data.ProtocolOne;
            this.protocol_one_detail = data.ProtocolOneDetail;
            this.baudrate_one = data.BaudRateOne;
            this.protocol_two_detail = data.ProtocolTwoDetail;
            this.protocol_two = data.ProtocolTwo;
            this.baudrate_two = data.BaudRateTwo;
            this.is_upload = data.IsUpload;
            this.is_reset = data.IsReset;
            this.last_sync = data.LastSync;
            this.updated_date = DateTime.UtcNow;
        }
    }
}
