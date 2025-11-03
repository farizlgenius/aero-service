using System.ComponentModel.DataAnnotations;

namespace HIDAeroService.Entity
{
    public class NoMacBaseEntity
    {
        [Key]
        public int Id { get; set; }
        public string Uuid { get; set; } = Guid.NewGuid().ToString();
        public int LocationId { get; set; } = 1;
        public string LocationName { get; set; } = "Main Location";
        public bool IsActive { get; set; } = true;
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
