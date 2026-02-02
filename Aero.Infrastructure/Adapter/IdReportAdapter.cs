using System;
using Aero.Domain.Interfaces;
using HID.Aero.ScpdNet.Wrapper;

namespace Aero.Infrastructure.Adapter;

public sealed class IdReportAdapter(SCPReplyMessage.SCPReplyIDReport id) : IIdreport
{
    public short device_id => id.device_id;
    public short device_ver => id.device_ver;

    public short sft_rev_major => id.sft_rev_major;
    public short sft_rev_minor => id.sft_rev_minor;

    public int serial_number => id.serial_number;

    public int ram_size => id.ram_size;
    public int ram_free => id.ram_free;

    public int e_sec => id.e_sec;

    public int db_max => id.db_max;
    public int db_active => id.db_active;

    public byte dip_switch_pwrup => id.dip_switch_pwrup;
    public byte dip_switch_current => id.dip_switch_current;

    public short scp_id => id.scp_id;
    public short firmware_advisory => id.firmware_advisory;

    public short scp_in_1 => id.scp_in_1;
    public short scp_in_2 => id.scp_in_2;
    public short scp_in_3 => id.scp_in_3;

    public int adb_max => id.adb_max;
    public int adb_active => id.adb_active;

    public int bio1_max => id.bio1_max;
    public int bio1_active => id.bio1_active;

    public int bio2_max => id.bio2_max;
    public int bio2_active => id.bio2_active;

    public short nOemCode => id.nOemCode;

    public byte config_flags => id.config_flags;

    public byte[] mac_addr => id.mac_addr;

    public byte tls_status => id.tls_status;
    public byte oper_mode => id.oper_mode;

    public int cumulative_bld_cnt => id.cumulative_bld_cnt;

    public byte hardware_id => id.hardware_id;
    public byte hardware_revision => id.hardware_revision;
    public int hardware_component_id => id.hardware_component_id;
}
