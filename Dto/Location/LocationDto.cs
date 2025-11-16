using System.ComponentModel.DataAnnotations;

namespace HIDAeroService.DTO.Location
{
    public class LocationDto : NoMacBaseDto
    {
        public string Description { get; set; } = string.Empty;
    }
}
