using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace HIDAeroService.Entity
{
    public class ar_base_entity
    {
        [Key]
        public int id { get; set; }
        public DateTime created_date { get; set; }
        public DateTime updated_date { get; set; }

    }
}
