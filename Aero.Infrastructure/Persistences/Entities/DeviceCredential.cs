using System.ComponentModel.DataAnnotations;

namespace Aero.Infrastructure.Persistences.Entities
{
    public sealed class DeviceCredential
    {
        [Key]
        public int id { get; set; }
        public int device_id { get; set; } 
        public short credential_id { get; set; }
        public Device hardware { get; set; }
        public Credential credential { get; set; }
    }
}
