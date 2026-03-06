using System.ComponentModel.DataAnnotations;
using Aero.Application.Helpers;
using Aero.Application.Interfaces;

namespace Aero.Infrastructure.Persistences.Entities
{
    public sealed class IdReport
    {
        [Key]
        public int id { get; set; }
        public short device_id { get; set; }
        public short device_ver { get; set; }
        public short software_rev_major { get; set; }
        public short software_rev_minor { get; set; }
        public string firmware { get; set; } = string.Empty;
        public int serial_number { get; set; }
        public int ram_size { get; set; }
        public int ram_free { get; set; }
        public int e_sec { get; set; }
        public int db_max { get; set; }
        public int db_active { get; set; }
        public byte dip_switch_powerup { get; set; }
        public byte dip_switch_current { get; set; }
        public short scp_id { get; set; }
        public short firmware_advisory { get; set; }
        public short scp_in1 { get; set; }
        public short scp_in2 { get; set; }
        public short n_oem_code { get; set; }
        public byte config_flag { get; set; }
        public string mac { get; set; } = string.Empty;
        public byte tls_status { get; set; }
        public byte oper_mode { get; set; }
        public short scp_in3 { get; set; }
        public int cumulative_bld_cnt { get; set; }
        public string ip { get; set; } = string.Empty;
        public string port { get; set; } = string.Empty;
        public int location_id { get; set; }
        public Location location { get; set; }

        public IdReport(){}


        public IdReport(IScpReply message)
        {
            this.device_id = message.id.device_id;
            this.device_ver = message.id.device_ver;
            this.software_rev_major = message.id.sft_rev_major;
            this.software_rev_minor = message.id.sft_rev_minor;
            this.firmware = UtilitiesHelper.ParseFirmware(message.id.sft_rev_major, message.id.sft_rev_minor);
            this.serial_number = message.id.serial_number;
            this.ram_size = message.id.ram_size;
            this.ram_free = message.id.ram_free;
            this.e_sec = message.id.e_sec;
            this.db_max = message.id.db_max;
            this.db_active = message.id.db_active;
            this.dip_switch_powerup = message.id.dip_switch_pwrup;
            this.dip_switch_current = message.id.dip_switch_current;
            this.scp_id = message.id.scp_id;
            this.firmware_advisory = message.id.firmware_advisory;
            this.scp_in1 = message.id.scp_in_1;
            this.scp_in2 = message.id.scp_in_2;
            this.n_oem_code = message.id.nOemCode;
            this.config_flag = message.id.config_flags;
            this.mac = UtilitiesHelper.ByteToHexStr(message.id.mac_addr);
            this.tls_status = message.id.tls_status;
            this.oper_mode = message.id.oper_mode;
            this.scp_in3 = message.id.scp_in_3;
            this.cumulative_bld_cnt = message.id.cumulative_bld_cnt;
            this.location_id = 1;
        }

        public void Update(IScpReply message)
        {
            this.device_id = message.id.device_id;
            this.device_ver = message.id.device_ver;
            this.software_rev_major = message.id.sft_rev_major;
            this.software_rev_minor = message.id.sft_rev_minor;
            this.firmware = UtilitiesHelper.ParseFirmware(message.id.sft_rev_major, message.id.sft_rev_minor);
            this.serial_number = message.id.serial_number;
            this.ram_size = message.id.ram_size;
            this.ram_free = message.id.ram_free;
            this.e_sec = message.id.e_sec;
            this.db_max = message.id.db_max;
            this.db_active = message.id.db_active;
            this.dip_switch_powerup = message.id.dip_switch_pwrup;
            this.dip_switch_current = message.id.dip_switch_current;
            this.scp_id = message.id.scp_id;
            this.firmware_advisory = message.id.firmware_advisory;
            this.scp_in1 = message.id.scp_in_1;
            this.scp_in2 = message.id.scp_in_2;
            this.n_oem_code = message.id.nOemCode;
            this.config_flag = message.id.config_flags;
            this.mac = UtilitiesHelper.ByteToHexStr(message.id.mac_addr);
            this.tls_status = message.id.tls_status;
            this.oper_mode = message.id.oper_mode;
            this.scp_in3 = message.id.scp_in_3;
            this.cumulative_bld_cnt = message.id.cumulative_bld_cnt;
            this.location_id = 1;
        }
    }
}

