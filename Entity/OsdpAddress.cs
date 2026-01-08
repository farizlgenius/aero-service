using System.ComponentModel.DataAnnotations;

namespace HIDAeroService.Entity
{
    public sealed class OsdpAddress
    {
        [Key]
        public int id { get; set; }
        public short value { get; set; }
        public string name { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
    }
}
