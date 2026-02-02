using System;
using Aero.Application.Helpers;
using Aero.Application.Interfaces;
using Aero.Infrastructure.Data.Entities;
using HID.Aero.ScpdNet.Wrapper;

namespace Aero.Infrastructure.Mapper;

public static class IdReportMapper
{
      public static IdReport ToEf(IScpReply message)
      {
            return new Aero.Infrastructure.Data.Entities.IdReport
            {
                  device_id = message.id.device_id,
                  device_ver = message.id.device_ver,
                  software_rev_major = message.id.sft_rev_major,
                  software_rev_minor = message.id.sft_rev_minor,
                  firmware = UtilitiesHelper.ParseFirmware(message.id.sft_rev_major, message.id.sft_rev_minor),
                  serial_number = message.id.serial_number,
                  ram_size = message.id.ram_size,
                  ram_free = message.id.ram_free,
                  e_sec = message.id.e_sec,
                  db_max = message.id.db_max,
                  db_active = message.id.db_active,
                  dip_switch_powerup = message.id.dip_switch_pwrup,
                  dip_switch_current = message.id.dip_switch_current,
                  scp_id = message.id.scp_id,
                  firmware_advisory = message.id.firmware_advisory,
                  scp_in1 = message.id.scp_in_1,
                  scp_in2 = message.id.scp_in_2,
                  scp_in3 = message.id.scp_in_3,
                  n_oem_code = message.id.nOemCode,
                  config_flag = message.id.config_flags,
                  mac = UtilitiesHelper.ByteToHexStr(message.id.mac_addr),
                  tls_status = message.id.tls_status,
                  oper_mode = message.id.oper_mode,
                  cumulative_bld_cnt = message.id.cumulative_bld_cnt,
                  port = "",
                  ip = ""
            };
      }

      public static Aero.Domain.Entities.IdReport ToDomain(IScpReply message,IScpCommand scp,short id)
      {
            return new Aero.Domain.Entities.IdReport
            {
                  DeviceId = message.id.device_id,
                  DeviceVer = message.id.device_ver,
                  SoftwareRevMajor = message.id.sft_rev_major,
                  SoftwareRevMinor = message.id.sft_rev_minor,
                  Firmware = UtilitiesHelper.ParseFirmware(message.id.sft_rev_major, message.id.sft_rev_minor),
                  SerialNumber = message.id.serial_number,
                  RamSize = message.id.ram_size,
                  RamFree = message.id.ram_free,
                  ESec = UtilitiesHelper.UnixToDateTime(message.id.e_sec),
                  DbMax = message.id.db_max,
                  DbActive = message.id.db_active,
                  DipSwitchPowerup = message.id.dip_switch_pwrup,
                  DipSwitchCurrent = message.id.dip_switch_current,
                  ScpId = scp.SetScpId(message.id.scp_id, id) ? id : message.id.scp_id,
                  FirmwareAdvisory = message.id.firmware_advisory,
                  ScpIn1 = message.id.scp_in_1,
                  ScpIn2 = message.id.scp_in_2,
                  NOemCode = message.id.nOemCode,
                  ConfigFlag = message.id.config_flags,
                  Mac = UtilitiesHelper.ByteToHexStr(message.id.mac_addr),
                  TlsStatus = message.id.tls_status,
                  OperMode = message.id.oper_mode,
                  ScpIn3 = message.id.scp_in_3,
                  CumulativeBldCnt = message.id.cumulative_bld_cnt,
                  Port = "",
                  Ip = ""
            };
      }

      public static void Update(Aero.Infrastructure.Data.Entities.IdReport en,IScpReply message)
      {
            en.device_id = message.id.device_id;
            en.device_ver = message.id.device_ver;
            en.software_rev_major = message.id.sft_rev_major;
            en.software_rev_minor = message.id.sft_rev_minor;
            en.firmware = UtilitiesHelper.ParseFirmware(message.id.sft_rev_major, message.id.sft_rev_minor);
            en.serial_number = message.id.serial_number;
            en.ram_size = message.id.ram_size;
            en.ram_free = message.id.ram_free;
            en.e_sec = message.id.e_sec;
            en.db_max = message.id.db_max;
            en.db_active = message.id.db_active;
            en.dip_switch_powerup = message.id.dip_switch_pwrup;
            en.dip_switch_current = message.id.dip_switch_current;
            //iDReport.hardware_id = command.SetScpId(message.id.scp_id, id) ? id : message.id.scp_id;
            en.firmware_advisory = message.id.firmware_advisory;
            en.scp_in1 = message.id.scp_in_1;
            en.scp_in2 = message.id.scp_in_2;
            en.n_oem_code = message.id.nOemCode;
            en.config_flag = message.id.config_flags;
            //iDReport.mac = UtilityHelper.ByteToHexStr(message.id.mac_addr);
            en.tls_status = message.id.tls_status;
            en.oper_mode = message.id.oper_mode;
            en.scp_in3 = message.id.scp_in_3;
            en.cumulative_bld_cnt = message.id.cumulative_bld_cnt;
            en.port = "";
            en.ip = "";
      }


}
