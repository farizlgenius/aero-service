namespace Aero.Application.DTOs
{
    public sealed class ChangeDoorModeDto
    {
        public string Mac { get; set; } = string.Empty;
        public short ComponentId { get; set; }
        public short Mode { get; set; }
    }
}
