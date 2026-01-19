namespace Aero.Application.DTOs
{
    public class HardwareStatus
    {
        public string Mac { get; set; } = string.Empty;
        public short ComponentId { get; set; }
        public short Status { get; set; }
    }
}
