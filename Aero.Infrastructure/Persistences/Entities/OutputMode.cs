using System.ComponentModel.DataAnnotations;

namespace Aero.Infrastructure.Persistences.Entities
{
    public sealed class OutputMode 
    {
        [Key]
        public int id { get; set; }
        public short value { get; set; }
        public short offline_mode { get; set; }
        public short relay_mode { get; set; }
        public string description { get; set; } = string.Empty;

    }
}
