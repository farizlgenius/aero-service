namespace Aero.Application.DTOs
{
    public sealed class ToggleControlPointDto
    {
        public string Mac { get; set; } = string.Empty;
        public short ComponentId { get; set; }
        public short Command { get; set; }
    }
}
