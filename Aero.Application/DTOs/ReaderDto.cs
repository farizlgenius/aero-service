using Aero.Domain.Entities;
using Aero.Domain.Enums;

namespace Aero.Application.DTOs
{
    public sealed class ReaderDto : BaseDomain
    {
        public short ModuleId { get; set; }
        public short ReaderNo { get; set; }
        public short DataFormat { get; set; } = 0x01;
        public short KeypadMode { get; set; } = 2;
        public short LedDriveMode { get; set; }
        public DoorDirection Direction {get; set;} = DoorDirection.IN;
        public bool OsdpFlag { get; set; }
        public short OsdpBaudrate { get; set; } = 0x01;
        public short OsdpDiscover { get; set; } = 0x08;
        public short OsdpTracing { get; set; } = 0x10;
        public short OsdpAddress { get; set; }
        public short OsdpSecureChannel { get; set; }
    }
}
