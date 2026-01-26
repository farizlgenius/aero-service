using Aero.Domain.Enums;

namespace Aero.Infrastructure.Data.Entities
{
    public sealed class Reader : BaseEntity
    {
        public short module_id { get; set; }
        public Module module { get; set; }
        public Door door { get; set; }
        public short reader_no { get; set; }
        public short data_format { get; set; } = 0x01;
        public short keypad_mode { get; set; } = 2;
        public short led_drive_mode { get; set; }
        public DoorDirection direction { get; set; }    
        public bool osdp_flag { get; set; }
        public short osdp_baudrate { get; set; } = 0x01;
        public short osdp_discover { get; set; } = 0x08;
        public short osdp_tracing { get; set; } = 0x10;
        public short osdp_address { get; set; }
        public short osdp_secure_channel { get; set; }
    }
}
