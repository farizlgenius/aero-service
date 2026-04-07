using System.ComponentModel.DataAnnotations;

namespace Aero.Infrastructure.Persistences.Entities
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
