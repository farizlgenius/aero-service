using System.ComponentModel.DataAnnotations;

namespace Aero.Infrastructure.Persistences.Entities
{
    public sealed class DoorAccessControlFlag
    {
        [Key]
        public int id { get; set; }
        public string name { get; set; } = string.Empty;
        public int value { get; set; }
        public string description { get; set; } = string.Empty;
    }
}
