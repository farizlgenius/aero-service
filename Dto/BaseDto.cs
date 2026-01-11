using AeroService.DTO.Location;
using System.ComponentModel.DataAnnotations;

namespace AeroService.DTO
{
    public class BaseDto
    {
        public string Uuid { get; set; } = Guid.NewGuid().ToString();
        public short ComponentId { get; set; }
        public string HardwareName { get; set; } = string.Empty;
        public string Mac { get; set; } = string.Empty;
        public short LocationId { get; set; }
        public bool IsActive { get; set; }
    }
}
