using System.ComponentModel.DataAnnotations;

namespace HIDAeroService.Entity
{
    public sealed class OutputOfflineMode
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public short Value { get; set; }
        public string Description { get; set; }
    }
}
