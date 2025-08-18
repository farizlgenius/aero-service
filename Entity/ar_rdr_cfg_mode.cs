using System.ComponentModel.DataAnnotations;

namespace HIDAeroService.Entity
{
    public class ar_rdr_cfg_mode
    {
        [Key]
        public int id { get; set; }
        public string name { get; set; }
        public short value { get; set; }
        public string description { get; set; }

    }
}
