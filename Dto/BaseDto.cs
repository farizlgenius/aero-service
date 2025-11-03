using System.ComponentModel.DataAnnotations;

namespace HIDAeroService.DTO
{
    public class BaseDto
    {
        public string Uuid { get; set; } = Guid.NewGuid().ToString();
        public short ComponentId { get; set; }
        public string MacAddress { get; set; }
        public int LocationId { get; set; }
        public string LocationName { get; set; }
        public bool IsActive { get; set; }
    }
}
