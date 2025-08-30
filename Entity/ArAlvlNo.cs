using System.ComponentModel.DataAnnotations;

namespace HIDAeroService.Entity
{
    public class ArAlvlNo : ArBaseEntity
    {
        public short AccessLevelNo { get; set; }
        public bool IsAvailable { get; set; }
    }
}
