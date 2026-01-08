using System.ComponentModel.DataAnnotations;

namespace HIDAeroService.Entity
{
    public sealed  class OccupancyControl
    {
        [Key]
        public int id { get; set; }
        public string name { get; set; } = string.Empty;
        public short value { get; set; }
        public string description { get; set; } = string.Empty;
    }
}
