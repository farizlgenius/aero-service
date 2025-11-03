using HIDAeroService.Entity.Interface;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace HIDAeroService.Entity
{
    public class BaseEntity : IMac,IComponentId,ILocation,IActivatable
    {
        [Key]
        public int Id { get; set; }
        public string Uuid { get; set; } = Guid.NewGuid().ToString();
        public short ComponentId { get; set; }
        public string MacAddress { get; set; } = string.Empty;
        public int LocationId { get; set; } = 1;
        public string LocationName { get; set; } = "Main Location";
        public bool IsActive { get; set; } = true;
        public DateTime CreatedDate { get; set; } 
        public DateTime UpdatedDate { get; set; }

    }
}
