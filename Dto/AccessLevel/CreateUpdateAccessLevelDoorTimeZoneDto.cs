namespace HIDAeroService.DTO.AccessLevel
{
    public sealed class CreateUpdateAccessLevelDoorTimeZoneDto
    {
        public short DoorId { get; set; }
        public string DoorName { get; set; }
        public string DoorMacAddress { get; set; }
        public string TimeZoneName { get; set; }
        public short TimeZoneId { get; set; }
    }
}
