using System.ComponentModel.DataAnnotations;

namespace HIDAeroService.Entity
{
    public class ArAcrNo : ArBaseEntity
    {
        public string ScpMac { get; set; }
        public short SioNo { get; set; }
        public short AcrNo { get; set; }
        public bool IsAvailable { get; set; }
    }
}
