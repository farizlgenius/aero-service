using HIDAeroService.DTO.Acr;
using HIDAeroService.DTO.TimeZone;

namespace HIDAeroService.DTO.AccessLevel
{
    public sealed class AccessLevelDoorTimeZoneDto
    {
        public DoorDto Doors { get; set; }
        public TimeZoneDto TimeZone { get; set; }
    }
}
