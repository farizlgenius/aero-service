using System.ComponentModel.DataAnnotations;

namespace Aero.Infrastructure.Persistence.Entities
{
    public sealed class MonitorGroupCommand
    {
        [Key]
        public int id { get; set; }
        public string name { get; set; } = string.Empty;
        public short value { get; set; }
        public string description { get; set; } = string.Empty;

    }
}
