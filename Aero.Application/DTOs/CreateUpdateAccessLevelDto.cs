using Aero.Application.Interfaces;

namespace Aero.Application.DTOs
{
    public sealed class CreateUpdateAccessLevelDto : NoMacBaseDto,IComponentId
    {
        public string Name { get; set; }
        public short component_id { get; set; }
        public List<CreateUpdateAccessLevelDoorTimeZoneDto> CreateUpdateAccessLevelDoorTimeZoneDto { get; set; } = new List<CreateUpdateAccessLevelDoorTimeZoneDto>();
    }
}
