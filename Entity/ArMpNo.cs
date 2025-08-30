using System.ComponentModel.DataAnnotations;

namespace HIDAeroService.Entity
{
    public sealed class ArMpNo : ArBaseEntity
    {
        public string ScpMac { get; set; }
        public short SioNo { get; set; }
        public short MpNo { get; set; }
        public bool IsAvailable { get; set; }
    }
}
