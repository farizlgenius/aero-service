using System.ComponentModel.DataAnnotations;

namespace Aero.Infrastructure.Data.Entities
{
    public sealed class OperatorLocation
    {
        [Key]
        public int id { get; set; } 
        public short location_id { get; set; }
        public Location location { get; set; }
        public short operator_id { get; set; }
        public Operator @operator { get; set; }
    }
}
