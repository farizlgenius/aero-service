
using Aero.Application.Interfaces;

namespace Aero.Application.DTOs
{
    public sealed class AccessLevelDto : NoMacBaseDto,IComponentId
    {
        public string Name { get; set; } = string.Empty;
        public short component_id { get; set; }
        public List<AccessLevelDoorTimeZoneDto>? AccessLevelDoorTimeZoneDto { get; set; }

    }
}
