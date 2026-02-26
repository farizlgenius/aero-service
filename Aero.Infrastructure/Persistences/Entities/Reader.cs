using Aero.Domain.Entities;
using Aero.Domain.Enums;
using Aero.Domain.Interfaces;
using System.Net;

namespace Aero.Infrastructure.Persistences.Entities
{
    public sealed class Reader : BaseEntity,IMac
    {
        public int module_id { get; set; }
        public Module module { get; set; }
        public int door_id { get; set; }
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
        public string mac { get; set; } = string.Empty;

        public Reader(int module,int doorid,short reader,short datadormat,short keypad,short leddrivermode,DoorDirection direction,bool osdpflag,short baudrate,short discover,short tracing,short address,short secure,string mac,int location) : base(location) 
        {
            this.module_id = module;
            this.door_id = doorid;
            this.reader_no = reader;
            this.data_format = datadormat;
            this.keypad_mode = keypad;
            this.led_drive_mode = leddrivermode;
            this.direction = direction;
            this.osdp_flag = osdpflag;
            this.osdp_baudrate = baudrate;
            this.osdp_discover = discover;
            this.osdp_tracing = tracing;
            this.osdp_address = address;
            this.osdp_secure_channel = secure;
            this.mac = mac;


        }

        public void Update(Aero.Domain.Entities.Reader data)
        {
            this.module_id = data.ModuleId;
            this.door_id = data.DoorId;
            this.reader_no = data.ReaderNo;
            this.data_format = data.DataFormat;
            this.keypad_mode = data.KeypadMode;
            this.led_drive_mode = data.LedDriveMode;
            this.direction = data.Direction;
            this.osdp_flag = data.OsdpFlag;
            this.osdp_baudrate = data.OsdpBaudrate;
            this.osdp_discover = data.OsdpDiscover;
            this.osdp_tracing = data.OsdpTracing;
            this.osdp_address = data.OsdpAddress;
            this.osdp_secure_channel = data.OsdpSecureChannel;
            this.mac = data.Mac;
        }
    }
}
