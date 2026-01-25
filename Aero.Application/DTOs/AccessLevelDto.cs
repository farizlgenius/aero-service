
using Aero.Application.Interfaces;
using Aero.Domain.Entities;

namespace Aero.Application.DTOs
{
    public sealed class AccessLevelDto : NoMacBaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public short component_id { get; set; }
        public List<AccessLevelDoorTimeZoneDto>? AccessLevelDoorTimeZoneDto { get; set; }

    }
}
