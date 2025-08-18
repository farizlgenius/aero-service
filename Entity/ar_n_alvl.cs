using System.ComponentModel.DataAnnotations;

namespace HIDAeroService.Entity
{
    public class ar_n_alvl
    {
        [Key]
        public int id { get; set; }
        public short alvl_number { get; set; }
        public bool is_available { get; set; }
    }
}
