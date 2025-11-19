using HIDAeroService.DTO.Location;
using System.ComponentModel.DataAnnotations;

namespace HIDAeroService.DTO
{
    public class NoMacBaseDto
    {
        public string Uuid { get; set; } = Guid.NewGuid().ToString();
        public short LocationId { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
