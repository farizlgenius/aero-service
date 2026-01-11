namespace AeroService.DTO.AccessLevel
{
    public sealed class CreateUpdateAccessLevelDoorTimeZoneDto
    {
        public short DoorId { get; set; }
        public string DoorName { get; set; } = string.Empty;
        public string DoorMacAddress { get; set; } = string.Empty;
        public string TimeZoneName { get; set; } = string.Empty;
        public short TimeZoneId { get; set; }
    }
}
