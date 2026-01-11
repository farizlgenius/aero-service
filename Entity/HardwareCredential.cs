using System.ComponentModel.DataAnnotations;

namespace AeroService.Entity
{
    public sealed class HardwareCredential
    {
        [Key]
        public int id { get; set; }
        public string hardware_mac { get; set; } = string.Empty;
        public short hardware_credential_id { get; set; }
        public Hardware hardware { get; set; }
        public Credential credential { get; set; }
    }
}
