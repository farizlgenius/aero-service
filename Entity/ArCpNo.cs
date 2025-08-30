using System.ComponentModel.DataAnnotations;

namespace HIDAeroService.Entity
{
    public sealed class ArCpNo : ArBaseEntity
    {
        public string ScpMac { get; set; }
        public short SioNo { get; set; }
        public short CpNo { get; set; }
        public bool IsAvailable { get; set; }
    }
}
