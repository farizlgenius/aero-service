using System;

namespace Aero.Domain.Entities;

public class NoMacBaseEntity 
{
            public string Uuid { get; set; } = Guid.NewGuid().ToString();
        public short ComponentId { get; set; }
        public string HardwareName { get; set; } = string.Empty;
        public short LocationId { get; set; }
        public bool IsActive { get; set; }
}
