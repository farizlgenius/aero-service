using System.ComponentModel.DataAnnotations;

namespace HIDAeroService.Entity
{
    public sealed class ar_ip_mode
    {
        [Key]
        public int id { get; set; }
        public string name { get; set; }
        public short value { get; set; }
        public string description { get; set; }
    }
}
