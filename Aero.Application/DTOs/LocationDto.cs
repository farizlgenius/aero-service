using System.ComponentModel.DataAnnotations;

namespace Aero.Application.DTOs
{
    public sealed class LocationDto 
    {
        public string Uuid { get; set; } = Guid.NewGuid().ToString();
        public short ComponentId { get; set; }
        public string LocationName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
    }
}
