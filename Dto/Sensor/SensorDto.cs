namespace AeroService.DTO.Sensor
{
    public sealed class SensorDto : BaseDto
    {
        public short ModuleId { get; set; }
        public short InputNo { get; set; }
        public short InputMode { get; set; }
        public short Debounce { get; set; }
        public short HoldTime { get; set; }
        public short DcHeld { get; set; }
    }
}
