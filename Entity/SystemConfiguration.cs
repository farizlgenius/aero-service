using System.ComponentModel.DataAnnotations;

namespace HIDAeroService.Entity
{
    public sealed class SystemConfiguration 
    {
        [Key]
        public int Id { get; set; }
        public short nPorts { get; set; }
        public short nScp { get; set; }
        public short nChannelId { get; set; }
        public short cType { get; set; }
        public short cPort { get; set; }

    }
}
