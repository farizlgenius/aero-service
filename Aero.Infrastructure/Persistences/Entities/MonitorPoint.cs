using Aero.Domain.Interfaces;

namespace Aero.Infrastructure.Persistences.Entities
{
    public sealed class MonitorPoint : BaseEntity,IDriverId,IMac
    {
        public short driver_id {get; set;}
        public string name { get; set; } = string.Empty;
        public int module_id { get; set; }
        public Module module { get; set; }
        public short input_no { get; set; }
        public short input_mode { get; set; }
        public string input_mode_detail { get; set; } = string.Empty;
        public short debounce { get; set; }
        public short holdtime { get; set; }
        public short log_function { get; set; } = 1;
        public string log_function_detail { get; set; } = string.Empty;
        public short monitor_point_mode { get; set; } = -1;
        public string monitor_point_mode_detail { get; set; } = string.Empty;
        public short delay_entry { get; set; } = -1;
        public short delay_exit { get; set; } = -1;
        public bool is_mask { get; set; }
        public string mac { get; set; } = string.Empty;

        public MonitorPoint(Aero.Domain.Entities.MonitorPoint data) : base(data.LocationId)
        {
            this.driver_id = data.DriverId;
            this.name = data.Name;
            this.module_id = data.ModuleId;
            this.input_no = data.InputNo;
            this.input_mode = data.InputMode;
            this.input_mode_detail = data.InputModeDetail;
            this.debounce = data.Debounce;
            this.holdtime = data.HoldTime;
            this.log_function = data.LogFunction;
            this.log_function_detail = data.LogFunctionDetail;
            this.monitor_point_mode = data.MonitorPointMode;
            this.monitor_point_mode_detail = data.MonitorPointModeDetail;
            this.delay_entry = data.DelayEntry;
            this.delay_exit = data.DelayExit;
            this.is_mask = data.IsMask;
            this.mac = data.Mac;
        }

        public void Update(Aero.Domain.Entities.MonitorPoint data) 
        {
            this.driver_id = data.DriverId;
            this.name = data.Name;
            this.module_id = data.ModuleId;
            this.input_no = data.InputNo;
            this.input_mode = data.InputMode;
            this.input_mode_detail = data.InputModeDetail;
            this.debounce = data.Debounce;
            this.holdtime = data.HoldTime;
            this.log_function = data.LogFunction;
            this.log_function_detail = data.LogFunctionDetail;
            this.monitor_point_mode = data.MonitorPointMode;
            this.monitor_point_mode_detail = data.MonitorPointModeDetail;
            this.delay_entry = data.DelayEntry;
            this.delay_exit = data.DelayExit;
            this.is_mask = data.IsMask;
            this.mac = data.Mac;
        }
    }
}
