namespace HIDAeroService.Entity
{
    public sealed class MonitorPoint : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public short ModuleId { get; set; }
        public Module Module { get; set; }
        public short InputNo { get; set; }
        public short InputMode { get; set; }
        public short Debounce { get; set; }
        public short HoldTime { get; set; }
        public short LogFunction { get; set; } = -1;
        public short MonitorPointMode { get; set; } = -1;
        public short DelayEntry { get; set; } = -1;
        public short DelayExit { get; set; } = -1;
        public bool IsMask { get; set; }
    }
}
