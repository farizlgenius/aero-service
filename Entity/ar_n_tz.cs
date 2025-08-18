using System.ComponentModel.DataAnnotations;

namespace HIDAeroService.Entity
{
    public sealed class ar_n_tz
    {
        [Key]
        public int id { get; set; }
        public short tz_number { get; set; }
        public bool is_available { get; set; }
    }
}
