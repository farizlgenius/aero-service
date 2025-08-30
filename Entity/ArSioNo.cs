using System.ComponentModel.DataAnnotations;

namespace HIDAeroService.Entity
{
    public sealed class ArSioNo : ArBaseEntity
    {
        public string ScpIp { get; set; }
        public string ScpMac { get; set; }
        public short SioNo { get; set; }
        public bool IsAvailable { get; set; }
        public short Port { get; set; }
    }
}
