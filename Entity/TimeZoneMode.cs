using HIDAeroService.Entity.Interface;
using System.ComponentModel.DataAnnotations;

namespace HIDAeroService.Entity
{
    public class TimeZoneMode 
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public short Value { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}
