using System.ComponentModel.DataAnnotations;

namespace HIDAeroService.Entity
{
    public class Location 
    {
        [Key]
        public int Id { get; set; } 
        public string Uuid { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
