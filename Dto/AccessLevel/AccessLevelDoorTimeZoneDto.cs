using AeroService.DTO.Acr;
using AeroService.DTO.TimeZone;

namespace AeroService.DTO.AccessLevel
{
    public sealed class AccessLevelDoorTimeZoneDto
    {
        public DoorDto Doors { get; set; }
        public TimeZoneDto TimeZone { get; set; }
    }
}
