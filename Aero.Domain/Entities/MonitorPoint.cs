using System;

namespace Aero.Domain.Entities;

public class MonitorPoint : BaseDomain
{
  public short DriverId {get; set;}
      public string Name { get; set; } = string.Empty;
        public short ModuleId { get; set; }
        public string ModuleDescription { get; set; } = string.Empty;
        public short InputNo { get; set; }
        public short InputMode { get; set; }
        public string InputModeDetail { get; set; } = string.Empty;
        public short Debounce { get; set; }
        public short HoldTime { get; set; }
        public short LogFunction { get; set; } = 1;
        public string LogFunctionDetail { get; set; } = string.Empty;
        public short MonitorPointMode { get; set; } = -1;
        public string MonitorPointModeDetail { get; set; } = string.Empty;
        public short DelayEntry { get; set; } = -1;
        public short DelayExit { get; set; } = -1;
        public bool IsMask { get; set; }
    public string Mac { get; set; } = string.Empty;
}
