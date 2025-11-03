using HIDAeroService.Entity.Interface;
using System.ComponentModel.DataAnnotations;

namespace HIDAeroService.Entity
{
    public sealed class Reader : BaseEntity
    {
        public short ModuleId { get; set; }
        public Module Module { get; set; }
        public Door Door { get; set; }
        public short ReaderNo { get; set; }
        public short DataFormat { get; set; } = 0x01;
        public short KeypadMode { get; set; } = 2;
        public short LedDriveMode { get; set; }
        public short Direction { get; set; }    
        public bool OsdpFlag { get; set; }
        public short OsdpBaudrate { get; set; } = 0x01;
        public short OsdpDiscover { get; set; } = 0x08;
        public short OsdpTracing { get; set; } = 0x10;
        public short OsdpAddress { get; set; }
        public short OsdpSecureChannel { get; set; }
    }
}
