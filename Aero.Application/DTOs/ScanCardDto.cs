namespace Aero.Application.DTOs
{
    public sealed class ScanCardDto
    {
        public string Mac { get; set; } = string.Empty;
        public short DoorId { get; set; }
    }
}
