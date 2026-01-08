using HIDAeroService.Entity.Interface;

namespace HIDAeroService.DTO.AccessLevel
{
    public sealed class AccessLevelDto : NoMacBaseDto,IComponentId
    {
        public string Name { get; set; }
        public short component_id { get; set; }
        public List<AccessLevelDoorTimeZoneDto> AccessLevelDoorTimeZoneDto { get; set; }

    }
}
