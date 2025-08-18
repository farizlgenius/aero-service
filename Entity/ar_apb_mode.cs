using System.ComponentModel.DataAnnotations;

namespace HIDAeroService.Entity
{
    public sealed class ar_apb_mode
    {
        [Key]
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public short value { get; set; }
    }
}
