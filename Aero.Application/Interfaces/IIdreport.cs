using System;

namespace Aero.Application.Interfaces;

public interface IIdreport
{
       //
            // Summary:
            //     identification of the replying device (1==Generic Scp type device)
             short device_id {get;}

            //
            // Summary:
            //     hardware version:
            //
            //     0==Scp2, 1==ScpC, 2==ScpE 3==Scp2Aes, 4==ScpCAes, 5==ScpEAes 7==EP2500, 8==EP1502,
            //     9 = EP1501
             short device_ver {get;}

            //
            // Summary:
            //     software revision, major
             short sft_rev_major{get;}

            //
            // Summary:
            //     software revision, (minor * 10 + build)
             short sft_rev_minor{get;}

            //
            // Summary:
            //     serial number
             int serial_number{get;}

            //
            // Summary:
            //     amount of ram installed
             int ram_size{get;}

            //
            // Summary:
            //     amount of ram available
             int ram_free{get;}

            //
            // Summary:
            //     current clock
             int e_sec{get;}

            //
            // Summary:
            //     access database size
             int db_max{get;}

            //
            // Summary:
            //     number of active records
             int db_active{get;}

            //
            // Summary:
            //     DIP switch at power-up: diagnostic
             byte dip_switch_pwrup{get;}

            //
            // Summary:
            //     DIP switch current value: diagnostic
             byte dip_switch_current{get;}

            //
            // Summary:
            //     the SCP's ID, as set by the host
             short scp_id{get;}

            //
            // Summary:
            //     Firmware Advisory
            //
            //     0==no firmware action,
            //
            //     1==must reset first,
            //
            //     2==starting load
             short firmware_advisory{get;}

            //
            // Summary:
            //     Scp local monitor "IN 1" state
             short scp_in_1{get;}

            //
            // Summary:
            //     Scp local monitor "IN 2" state
             short scp_in_2{get;}

            //
            // Summary:
            //     asset database, number or records created
             int adb_max{get;}

            //
            // Summary:
            //     asset database, number of records loaded
             int adb_active{get;}

            //
            // Summary:
            //     Bio-1 database, number or records created
             int bio1_max{get;}

            //
            // Summary:
            //     Bio-1 database, number of records loaded
             int bio1_active{get;}

            //
            // Summary:
            //     Bio-2 database, number or records created
             int bio2_max{get;}

            //
            // Summary:
            //     Bio-2 database, number of records loaded
             int bio2_active{get;}

            //
            // Summary:
            //     OEM Code assigned to the SCP
             short nOemCode{get;}

            //
            // Summary:
            //     Configuration flags. (Bit-0 = Needs CC_SCP_SCP configuration, SCP not yet known
            //     to driver)
             byte config_flags{get;}

            //
            // Summary:
            //     MAC Address, if applicable, LSB first.
             byte[] mac_addr {get;}

            //
            // Summary:
            //     TLS Status
             byte tls_status{get;}

            //
            // Summary:
            //     Current operating mode
             byte oper_mode{get;}

            //
            // Summary:
            //     Scp local monitor "IN 3" state
             short scp_in_3{get;}

            //
            // Summary:
            //     Cumulative build count
             int cumulative_bld_cnt{get;}

            //
            // Summary:
            //     Hardware ID
             byte hardware_id{get;}

            //
            // Summary:
            //     Hardware Revision
             byte hardware_revision{get;}

            //
            // Summary:
            //     Hardware Components ID
             int hardware_component_id{get;}
}
