namespace Aero.Application.DTOs
{
    public class ControlPointDto : BaseDto
    {
        public string Name { get; set; } = string.Empty;
        public short ModuleId { get; set; }
        public string ModuleDescription { get; set; } = string.Empty;
        public short OutputNo { get; set; }
        public short RelayMode { get; set; }
        public string RelayModeDescription { get; set; } = string.Empty;
        public short OfflineMode { get; set; }
        public string OfflineModeDescription { get; set; } = string.Empty;
        public short DefaultPulse { get; set; } = 1;
    }
}
