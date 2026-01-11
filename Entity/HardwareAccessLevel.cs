using System.ComponentModel.DataAnnotations;

namespace AeroService.Entity
{
    public sealed class HardwareAccessLevel
    {
        [Key]
        public int id { get; set; }
        public short hardware_accesslevel_id { get; set; }
        public AccessLevel access_level { get; set; }
        public string hardware_mac { get; set; } = string.Empty;
        public Hardware hardware { get; set; }
    }
}
