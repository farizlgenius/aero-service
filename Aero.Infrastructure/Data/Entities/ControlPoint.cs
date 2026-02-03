namespace Aero.Infrastructure.Data.Entities
{
    public sealed class ControlPoint : BaseEntity
    {
        public short cp_id {get; set;}
        public string name { get; set; } = string.Empty;
        public Module module { get; set; }
        public short module_id { get; set; }
        public string module_desc { get; set; } = string.Empty;
        public short output_no { get; set; }
        public short relay_mode { get; set; }
        public string relay_mode_desc { get; set; } = string.Empty;
        public short offline_mode { get; set; }
        public string offline_mode_desc { get; set; } = string.Empty;
        public short default_pulse { get; set; } = 1;
    }
}
