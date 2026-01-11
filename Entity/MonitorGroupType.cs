using System.ComponentModel.DataAnnotations;

namespace AeroService.Entity
{
    public sealed class MonitorGroupType
    {
        [Key]
        public int id { get; set; }
        public short value { get; set; }
        public string name { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
    }
}
