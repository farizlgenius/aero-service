using System.ComponentModel.DataAnnotations;

namespace HIDAeroService.Entity
{
    public class ArOpMode : ArBaseEntity
    {
        public short Value { get; set; }
        public string Description { get; set; }

    }
}
