

namespace Aero.Application.DTOs
{
    public class NoMacBaseDto
    {
        public string Uuid { get; set; } = Guid.NewGuid().ToString();
        public string HardwareName {get; set;} = string.Empty;
        public short LocationId { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
