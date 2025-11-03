namespace HIDAeroService.DTO.ControlPoint
{
    public class ControlPointDto : BaseDto
    {
        public string Name { get; set; } = string.Empty;
        public short ModuleId { get; set; }
        public short OutputNo { get; set; }
        public short RelayMode { get; set; }
        public short OfflineMode { get; set; }
        public short DefaultPulse { get; set; } = 1;
    }
}
