using System.ComponentModel.DataAnnotations;

namespace HIDAeroService.Entity
{
    public sealed class Component 
    {
        [Key]
        public int Id { get; set; }
        public short ModelNo { get; set; }
        public string Name { get; set; }
        public short nInput { get; set; }
        public short nOutput { get; set; }
        public short nReader { get; set; }

    }
}
