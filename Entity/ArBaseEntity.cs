using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace HIDAeroService.Entity
{
    public class ArBaseEntity
    {
        [Key]
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime UpdatedDate { get; set; } = DateTime.Now;

    }
}
