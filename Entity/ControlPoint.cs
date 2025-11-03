namespace HIDAeroService.Entity
{
    public sealed class ControlPoint : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public Module Module { get; set; }
        public short ModuleId { get; set; }
        public short OutputNo { get; set; }
        public short RelayMode { get; set; }
        public short OfflineMode { get; set; }
        public short DefaultPulse { get; set; } = 1;
    }
}
