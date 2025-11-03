using System.ComponentModel.DataAnnotations;

namespace HIDAeroService.Entity
{
    public sealed class HardwareAccessLevel
    {
        [Key]
        public int Id { get; set; }
        public short HardwareAccessLevelId { get; set; }
        public AccessLevel AccessLevel { get; set; }
        public string MacAddress { get; set; }
        public Hardware Hardware { get; set; }
    }
}
