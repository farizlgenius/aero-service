using System.ComponentModel.DataAnnotations;

namespace HIDAeroService.Entity
{
    public sealed class MonitorGroupType
    {
        [Key]
        public int Id { get; set; }
        public short Value { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
