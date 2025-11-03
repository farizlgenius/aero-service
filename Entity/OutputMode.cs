using System.ComponentModel.DataAnnotations;

namespace HIDAeroService.Entity
{
    public sealed class OutputMode 
    {
        [Key]
        public int Id { get; set; }
        public short Value { get; set; }
        public short OfflineMode { get; set; }
        public short RelayMode { get; set; }
        public string Description { get; set; }

    }
}
