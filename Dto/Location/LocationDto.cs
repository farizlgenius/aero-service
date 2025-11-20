using System.ComponentModel.DataAnnotations;

namespace HIDAeroService.DTO.Location
{
    public sealed class LocationDto 
    {
        public string Uuid { get; set; } = Guid.NewGuid().ToString();
        public short ComponentId { get; set; } = 1;
        public string LocationName { get; set; } = "Main Location";
        public string Description { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
    }
}
