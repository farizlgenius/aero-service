using System;

namespace Aero.Domain.Entities;

public sealed class Sensor : BaseDomain
{
    public int DeviceId { get; set; }
      public int ModuleId { get; set; }
    public int DoorId { get; set; }
        public short InputNo { get; set; }
        public short InputMode { get; set; }
        public short Debounce { get; set; }
        public short HoldTime { get; set; }
        public short DcHeld { get; set; }
}
