using System.ComponentModel.DataAnnotations;

namespace HIDAeroService.DTO.Location
{
    public class LocationDto 
    {
        public string Uuid { get; set; } = Guid.NewGuid().ToString();
        public short ComponentId { get; set; } = 1;
        public string LocationName { get; set; } = "Main Location";
        public string Description { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
