namespace AeroService.Entity
{
    public sealed class MonitorPoint : BaseEntity
    {
        public string name { get; set; } = string.Empty;
        public short module_id { get; set; }
        public Module module { get; set; }
        public short input_no { get; set; }
        public short input_mode { get; set; }
        public string input_mode_desc { get; set; } = string.Empty;
        public short debounce { get; set; }
        public short holdtime { get; set; }
        public short log_function { get; set; } = 1;
        public string log_function_desc { get; set; } = string.Empty;
        public short monitor_point_mode { get; set; } = -1;
        public string monitor_point_mode_desc { get; set; } = string.Empty;
        public short delay_entry { get; set; } = -1;
        public short delay_exit { get; set; } = -1;
        public bool is_mask { get; set; }
    }
}
