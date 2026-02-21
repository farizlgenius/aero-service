using System.ComponentModel.DataAnnotations;

namespace Aero.Infrastructure.Data.Entities
{
    public sealed class HardwareType
    {
        [Key]
        public int id { get; set; }
        public string name { get; set; } = string.Empty;
        public short component_id { get; set; }
        public string description { get; set; } = string.Empty;
    }
}
