using System.ComponentModel.DataAnnotations;

namespace AeroService.Entity
{
    public sealed class ModuleBaudrate
    {
        [Key]
        public int id { get; set; }
        public string name { get; set; } = string.Empty;    
        public int value { get; set; }
        public string description { get; set; } = string.Empty;
    }
}
