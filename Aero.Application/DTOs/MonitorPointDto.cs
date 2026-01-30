using Aero.Domain.Entities;

namespace Aero.Application.DTOs
{
    public sealed class MonitorPointDto : BaseEntity
    {
        public short MpId {get; set;}
        public string Name { get; set; } = string.Empty;
        public short ModuleId { get; set; }
        public string ModuleDescription { get; set; } = string.Empty;
        public short InputNo { get; set; }
        public short InputMode { get; set; }
        public string InputModeDescription { get; set; } = string.Empty;
        public short Debounce { get; set; }
        public short HoldTime { get; set; }
        public short LogFunction { get; set; } = 1;
        public string LogFunctionDescription { get; set; } = string.Empty;
        public short MonitorPointMode { get; set; } = -1;
        public string MonitorPointModeDescription { get; set; } = string.Empty;
        public short DelayEntry { get; set; } = -1;
        public short DelayExit { get; set; } = -1;
        public bool IsMask { get; set; }
    }

}
