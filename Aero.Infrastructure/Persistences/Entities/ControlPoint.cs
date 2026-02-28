using Aero.Domain.Interfaces;

namespace Aero.Infrastructure.Persistences.Entities
{
    public sealed class ControlPoint : BaseEntity, IDeviceId,IDriverId
    {
        public short driver_id {get; set;}
        public string name { get; set; } = string.Empty;
        public Module module { get; set; }
        public int module_id { get; set; }
        public string module_detail { get; set; } = string.Empty;
        public short output_no { get; set; }
        public short relaymode { get; set; }
        public string relaymode_detail { get; set; } = string.Empty;
        public short offlinemode { get; set; }
        public string offlinemode_detail { get; set; } = string.Empty;
        public short default_pulse { get; set; } = 1;
        public int device_id { get; set; }
        public Device device { get; set; }

        public ControlPoint(short driver,string name,int module_id,string module_detail,short output_no,short relaymode,string relaymode_detail,short offlinemode,string offlinemode_detail,short defaultpulse,int device,int location_id) : base(location_id) 
        {
            this.driver_id = driver;
            this.name = name;
            this.module_id = module_id;
            this.module_detail = module_detail;
            this.output_no = output_no;
            this.relaymode = relaymode;
            this.relaymode_detail = relaymode_detail;
            this.offlinemode = offlinemode;
            this.offlinemode_detail = offlinemode_detail;
            this.default_pulse = defaultpulse;
            this.device_id = device;
        }

        public ControlPoint(Aero.Domain.Entities.ControlPoint data) : base(data.LocationId)
        {
            this.driver_id = data.DriverId;
            this.name = data.Name;
            this.module_id = data.ModuleId;
            this.module_detail = data.ModuleDetail;
            this.output_no = data.OutputNo;
            this.relaymode = data.RelayMode;
            this.relaymode_detail = data.RelayModeDetail;
            this.offlinemode = data.OfflineMode;
            this.offlinemode_detail = data.OfflineModeDetail;
            this.default_pulse = data.DefaultPulse;
            this.device_id = data.DeviceId;
        }

        public void Update(Aero.Domain.Entities.ControlPoint data) 
        {
            this.name = data.Name;
            this.module_id = data.ModuleId;
            this.module_detail = data.ModuleDetail;
            this.output_no = data.OutputNo;
            this.relaymode = data.RelayMode;
            this.relaymode_detail = data.RelayModeDetail;
            this.offlinemode = data.OfflineMode;
            this.offlinemode_detail = data.OfflineModeDetail;
            this.default_pulse = data.DefaultPulse;
            this.updated_date = DateTime.Now;
        }

        
    }
}
