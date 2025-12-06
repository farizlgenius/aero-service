using System.ComponentModel.DataAnnotations;

namespace HIDAeroService.Entity
{
    public sealed class HardwareCredential
    {
        [Key]
        public int Id { get; set; }
        public string MacAddress { get; set; } = string.Empty;
        public short HardwareCredentialId { get; set; }
        public Hardware Hardware { get; set; }
        public Credential Credential { get; set; }
    }
}
