using System;

namespace Aero.Domain.Entities;

public sealed class CreateUpdateAccessLevelDoorTimeZone
{
        public short DoorId { get; set; }
        public short AcrId { get; set; }
        public string DoorMac { get; set; } = string.Empty;
        public short TimezoneId { get; set; }
}
