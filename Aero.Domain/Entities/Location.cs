using System;
using Aero.Domain.Interface;

namespace Aero.Domain.Entities;

public sealed class Location
{        public short ComponentId { get; set; }
        public string LocationName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
}
