using System;

namespace Aero.Domain.Entities;

public sealed class RequestExit : BaseDomain
{
      public short ModuleId { get; set; }
        public short InputNo { get; set; }
        public short InputMode { get; set; }
        public short Debounce { get; set; }
        public short HoldTime { get; set; }
        public short MaskTimeZone { get; set; } = 0;
}
