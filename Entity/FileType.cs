using System.ComponentModel.DataAnnotations;

namespace AeroService.Entity
{
    public sealed class FileType
    {
        [Key]
        public int id { get; set; }
        public string name { get; set; } = string.Empty;
        public short value { get; set; }
    }
}
