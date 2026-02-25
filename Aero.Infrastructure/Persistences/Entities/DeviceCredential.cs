using System.ComponentModel.DataAnnotations;

namespace Aero.Infrastructure.Persistences.Entities
{
    public sealed class DeviceCredential
    {
        [Key]
        public int id { get; set; }
        public string hardware_mac { get; set; } = string.Empty;
        public short hardware_credential_id { get; set; }
        public Device hardware { get; set; }
        public Credential credential { get; set; }
    }
}
