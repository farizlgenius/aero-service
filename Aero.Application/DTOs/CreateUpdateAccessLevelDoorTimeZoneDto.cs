namespace Aero.Application.DTOs
{
    public sealed class CreateUpdateAccessLevelDoorTimeZoneDto
    {
        public short DoorId { get; set; }
        public short AcrId {get; set;}
        public string DoorMac {get; set;} = string.Empty;
        public short TimezoneId { get; set; }
    }
}
