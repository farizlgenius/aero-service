using System.ComponentModel.DataAnnotations;

namespace HIDAeroService.Entity
{
    public sealed class OperatorLocation
    {
        [Key]
        public int Id { get; set; } 
        public short LocationId { get; set; }
        public Location Location { get; set; }
        public short OperatorId { get; set; }
        public Operator Operator { get; set; }
    }
}
