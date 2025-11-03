using System.ComponentModel.DataAnnotations;

namespace HIDAeroService.Entity
{
    public sealed class DoorAccessControlFlag
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Value { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}
