using HIDAeroService.Entity.Interface;
using System.ComponentModel.DataAnnotations;

namespace HIDAeroService.Entity
{
    public sealed class Hardware : BaseEntity
    {
        public string Name { get; set; }
        public string Model { get; set; }
        public ICollection<Module> Module { get; set; }
        public ICollection<HardwareCredential> HardwareCredentials { get; set; }
        public ICollection<HardwareAccessLevel> HardwareAccessLevels { get; set; }
        public string IpAddress { get; set; }
        public string SerialNumber { get; set; }
        public bool IsUpload { get; set; } = false;
        public bool IsReset { get; set; } = false;
        public DateTime LastSync { get; set; }
    }
}
