using System.ComponentModel.DataAnnotations;

namespace HIDAeroService.DTO
{
    public class NoMacBaseDto
    {
        public string Uuid { get; set; } = Guid.NewGuid().ToString();
        public int LocationId { get; set; } = 1;
        public string LocationName { get; set; } = "Main Location";
        public bool IsActive { get; set; } = true;
    }
}
