using System;
using Aero.Application.Helpers;
using Aero.Application.Interfaces;
using Aero.Infrastructure.Data.Entities;
using HID.Aero.ScpdNet.Wrapper;

namespace Aero.Infrastructure.Mapper;

public static class IdReportMapper
{
      public static IdReport ToEf(Aero.Domain.Entities.IdReport report)
      {
            return new Aero.Infrastructure.Data.Entities.IdReport
            {
                  device_id = report.DeviceId,
                  device_ver = report.DeviceVer,
                  software_rev_major = report.SoftwareRevMajor,
                  software_rev_minor = report.SoftwareRevMinor,
                  
            };
      }

      public static Aero.Domain.Entities.IdReport ToDomain(SCPReplyMessage message,IScpCommand scp,short id)
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

      public static void Update(Aero.Domain.Entities.IdReport iDReport,SCPReplyMessage message)
      {
            iDReport.DeviceId = message.id.device_id;
            iDReport.DeviceVer = message.id.device_ver;
            iDReport.SoftwareRevMajor = message.id.sft_rev_major;
            iDReport.SoftwareRevMinor = message.id.sft_rev_minor;
            iDReport.Firmware = UtilitiesHelper.ParseFirmware(message.id.sft_rev_major, message.id.sft_rev_minor);
            iDReport.SerialNumber = message.id.serial_number;
            iDReport.RamSize = message.id.ram_size;
            iDReport.RamFree = message.id.ram_free;
            iDReport.ESec = UtilitiesHelper.UnixToDateTime(message.id.e_sec);
            iDReport.DbMax = message.id.db_max;
            iDReport.DbActive = message.id.db_active;
            iDReport.DipSwitchPowerup = message.id.dip_switch_pwrup;
            iDReport.DipSwitchCurrent = message.id.dip_switch_current;
            //iDReport.hardware_id = command.SetScpId(message.id.scp_id, id) ? id : message.id.scp_id;
            iDReport.FirmwareAdvisory = message.id.firmware_advisory;
            iDReport.ScpIn1 = message.id.scp_in_1;
            iDReport.ScpIn2 = message.id.scp_in_2;
            iDReport.NOemCode = message.id.nOemCode;
            iDReport.ConfigFlag = message.id.config_flags;
            //iDReport.mac = UtilityHelper.ByteToHexStr(message.id.mac_addr);
            iDReport.TlsStatus = message.id.tls_status;
            iDReport.OperMode = message.id.oper_mode;
            iDReport.ScpIn3 = message.id.scp_in_3;
            iDReport.CumulativeBldCnt = message.id.cumulative_bld_cnt;
            iDReport.Port = "";
            iDReport.Ip = "";
      }


}
