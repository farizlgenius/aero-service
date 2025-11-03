namespace HIDAeroService.Entity
{
    public class RequestExit : BaseEntity
    {
        public Module Module { get; set; }
        public Door Door { get; set; }
        public short ModuleId { get; set; }
        public short InputNo { get; set; }
        public short InputMode { get; set; }
        public short Debounce { get; set; }
        public short HoldTime { get; set; }
        public short MaskTimeZone { get; set; } = 0;
    }
}
