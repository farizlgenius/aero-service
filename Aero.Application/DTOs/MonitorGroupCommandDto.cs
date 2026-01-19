namespace Aero.Application.DTOs
{
    public sealed class MonitorGroupCommandDto
    {
        public string Mac { get; set; } = string.Empty;
        public short ComponentId { get; set; }
        public short Command { get; set; }
        public short Arg { get; set; }
    }
}
