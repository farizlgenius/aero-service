using System.ComponentModel.DataAnnotations;

namespace HIDAeroService.Entity
{
    public class ar_op_mode
    {
        [Key]
        public int id { get; set; }
        public short value { get; set; }
        public string description { get; set; }

    }
}
