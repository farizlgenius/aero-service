using System;
using Aero.Application.DTOs;
using Aero.Application.Helpers;
using Aero.Application.Interfaces;
using Aero.Domain.Entities;
using Aero.Domain.Interface;
using Aero.Infrastructure.Data;
using Aero.Infrastructure.Mapper;
using HID.Aero.ScpdNet.Wrapper;
using Microsoft.EntityFrameworkCore;

namespace Aero.Infrastructure.Repositories;

public sealed class HwRepository(AppDbContext context, IScpCommand scp, IScpNotificationPublisher publisher) : IHwRepository
{
      public async Task<int> AddAsync(Hardware entity)
      {
            var ef = HardwareMapper.ToEf(entity);
            await context.hardware.AddAsync(ef);
            return await context.SaveChangesAsync();
      }

      public async Task<int> DeleteByComponentAsync(short component)
      {
            var ef = await context.hardware
            .Where(x => x.component_id == component)
            .OrderBy(x => x.component_id)
            .FirstOrDefaultAsync();

            if (ef is null) return 0;

            context.hardware.Remove(ef);
            return await context.SaveChangesAsync();
      }

      public async Task<int> DeleteByMacAsync(string mac)
      {
            var ef = await context.hardware
            .Where(x => x.mac.Equals(mac))
            .OrderBy(x => x.component_id)
            .FirstOrDefaultAsync();

            if (ef is null) return 0;

            context.hardware.Remove(ef);
            return await context.SaveChangesAsync();
      }

      public async Task<Hardware> GetByMacAsync(string mac)
      {
            var res = await context.hardware
            .Where(x => x.mac.Equals(mac))
            .Select(hardware => new Hardware
            {
                  // Base 
                  Uuid = hardware.uuid,
                  ComponentId = hardware.component_id,
                  Mac = hardware.mac,
                  LocationId = hardware.location_id,
                  IsActive = hardware.is_active,

                  // extend_desc
                  Name = hardware.name,
                  HardwareType = hardware.hardware_type,
                  HardwareTypeDescription = hardware.hardware_type_desc,
                  Firmware = hardware.firmware,
                  Ip = hardware.ip,
                  Port = hardware.port,
                  SerialNumber = hardware.serial_number,
                  IsReset = hardware.is_reset,
                  IsUpload = hardware.is_upload,
                  Modules = hardware.modules.Select(d => new Module
                  {
                        // Base 
                        Uuid = d.uuid,
                        ComponentId = d.component_id,
                        HardwareName = hardware.name,
                        Mac = hardware.mac,
                        LocationId = d.location_id,
                        IsActive = d.is_active,

                        // extend_desc
                        Model = d.model,
                        ModelDescription = d.model_desc,
                        Revision = d.revision,
                        SerialNumber = d.serial_number,
                        nHardwareId = d.n_hardware_id,
                        nHardwareIdDescription = d.n_hardware_id_desc,
                        nHardwareRev = d.n_hardware_rev,
                        nProductId = d.n_product_id,
                        nProductVer = d.n_product_ver,
                        nEncConfig = d.n_enc_config,
                        nEncConfigDescription = d.n_enc_config_desc,
                        nEncKeyStatus = d.n_enc_key_status,
                        nEncKeyStatusDescription = d.n_enc_key_status_desc,
                        Readers = null,
                        Sensors = null,
                        Strikes = null,
                        RequestExits = null,
                        MonitorPoints = null,
                        ControlPoints = null,
                        Address = d.address,
                        Port = d.port,
                        nInput = d.n_input,
                        nOutput = d.n_output,
                        nReader = d.n_reader,
                        Msp1No = d.msp1_no,
                        BaudRate = d.baudrate,
                        nProtocol = d.n_protocol,
                        nDialect = d.n_dialect,
                  }).ToList(),
                  PortOne = hardware.port_one,
                  ProtocolOne = hardware.protocol_one,
                  ProtocolOneDescription = hardware.protocol_one_desc,
                  PortTwo = hardware.port_two,
                  ProtocolTwoDescription = hardware.protocol_two_desc,
                  ProtocolTwo = hardware.protocol_two,
                  BaudRateOne = hardware.baudrate_one,
                  BaudRateTwo = hardware.baudrate_two,

            })
            .OrderBy(x => x.ComponentId)
            .FirstOrDefaultAsync();

            return res;

      }

      public async Task<int> UpdateSyncStatusByMacAsync(string mac)
      {
            var res = await context.hardware.
            Where(x => x.mac.Equals(mac))
            .OrderBy(x => x.component_id)
            .FirstOrDefaultAsync();

            if (res is null) return 0;

            res.updated_date = DateTime.UtcNow;
            res.last_sync = DateTime.UtcNow;
            res.is_upload = false;
            context.hardware.Update(res);
            return await context.SaveChangesAsync();
      }

      public async Task<int> UpdateVerifyHardwareCofigurationMyMacAsync(string mac, bool status)
      {
            var hardware = await context.hardware
            .Where(x => x.mac == mac)
            .FirstOrDefaultAsync();

            if (hardware is null) return 0;

            hardware.updated_date = DateTime.UtcNow;
            hardware.is_upload = status;

            context.hardware.Update(hardware);
            return await context.SaveChangesAsync();
      }

      public async Task<int> UpdateVerifyMemoryAllocateByComponentIdAsync(short component, bool isSync)
      {
            var hw = await context.hardware.
            Where(x => x.component_id == component)
            .OrderBy(x => x.component_id)
            .FirstOrDefaultAsync();

            if (hw is null) return 0;
            hw.is_reset = isSync;
            hw.updated_date = DateTime.UtcNow;
            context.hardware.Update(hw);
            return await context.SaveChangesAsync();
      }

      public async Task AssignIpAddressAsync(SCPReplyMessage message)
      {
            if (!await context.hardware.AnyAsync(x => x.component_id == (short)message.SCPId))
            {
                  var hw = context.hardware
                      .Where(x => x.component_id == message.SCPId)
                      .OrderBy(x => x.component_id)
                      .FirstOrDefault();

                  if (hw is null) return;


                  if (message.web_network is not null) hw.ip = UtilitiesHelper.IntegerToIp(message.web_network.cIpAddr);

                  context.hardware.Update(hw);
                  await context.SaveChangesAsync();

                  scp.GetWebConfigRead((short)message.SCPId, 3);

            }
            else
            {
                  var report = await context.id_report
                      .Where(x => x.scp_id == message.SCPId)
                      .OrderBy(x => x.id)
                      .FirstOrDefaultAsync();

                  if (report is null) return;

                  if (message.web_network is not null) report.ip = UtilitiesHelper.IntegerToIp(message.web_network.cIpAddr);

                  context.id_report.Update(report);
                  await context.SaveChangesAsync();

                  scp.GetWebConfigRead((short)message.SCPId, 3);
            }


      }

      public async Task AssignPort(SCPReplyMessage message)
      {
            if (!await context.hardware.AnyAsync(x => x.component_id == (short)message.SCPId))
            {
                  var hw = context.hardware
                      .Where(x => x.component_id == message.SCPId)
                      .OrderBy(x => x.component_id)
                      .FirstOrDefault();

                  if (hw is null) return;

                  if (message.web_host_comm_prim is not null)
                  {
                        if (message.web_host_comm_prim.ipclient is not null)
                        {
                              hw.port = message.web_host_comm_prim.ipclient.nPort.ToString();
                        }
                        else if (message.web_host_comm_prim.ipserver is not null)
                        {
                              hw.port = message.web_host_comm_prim.ipserver.nPort.ToString();
                        }
                  }
                ;

                  context.hardware.Update(hw);
                  await context.SaveChangesAsync();

                  var dto = await context.id_report
                      .AsNoTracking()
                      .Select(en => new IdReportDto()
                      {
                            ComponentId = en.scp_id,
                            SerialNumber = en.serial_number,
                            MacAddress = en.mac,
                            Ip = en.ip,
                            Port = en.port,
                            Firmware = en.firmware,
                            HardwareTypeDescription = "HID Aero",
                            HardwareType = 1,
                      })
                      .ToListAsync();

                  await publisher.IdReportNotifyAsync(dto);


            }
            else
            {
                  var report = await context.id_report
                      .Where(x => x.scp_id == message.SCPId)
                      .OrderBy(x => x.id)
                      .FirstOrDefaultAsync();

                  if (report is null) return;

                  if (message.web_host_comm_prim is not null)
                  {
                        if (message.web_host_comm_prim.ipclient is not null)
                        {
                              report.port = message.web_host_comm_prim.ipclient.nPort.ToString();
                        }
                        else if (message.web_host_comm_prim.ipserver is not null)
                        {
                              report.port = message.web_host_comm_prim.ipserver.nPort.ToString();
                        }
                  }
                ;

                  context.id_report.Update(report);
                  await context.SaveChangesAsync();

                  var dto = await context.id_report
                      .AsNoTracking()
                      .Select(en => new IdReportDto()
                      {
                            ComponentId = en.scp_id,
                            SerialNumber = en.serial_number,
                            MacAddress = en.mac,
                            Ip = en.ip,
                            Port = en.port,
                            Firmware = en.firmware,
                            HardwareTypeDescription = "HID Aero",
                            HardwareType = 1,
                      })
                      .ToListAsync();

                  await publisher.IdReportNotifyAsync(dto);
            }


      }

      public async Task HandleFoundHardware(SCPReplyMessage message)
      {
            if (await context.hardware.AnyAsync(x => x.mac.Equals(UtilitiesHelper.ByteToHexStr(message.id.mac_addr))))
            {
                  var hardware = await context.hardware
                      .FirstOrDefaultAsync(d => d.mac.Equals(UtilitiesHelper.ByteToHexStr(message.id.mac_addr)));

                  if (hardware is null) return;

                  if (!await MappingHardwareAndAllocateMemory(message.id.scp_id))
                  {
                        hardware.is_reset = true;
                  }
                  else
                  {
                        hardware.is_reset = false;
                  }

                  if (!await VerifyMemoryAllocateAsync(hardware.mac))
                  {
                        hardware.is_reset = true;
                  }
                  else
                  {
                        hardware.is_reset = false;
                  }

                  hardware.firmware = UtilitiesHelper.ParseFirmware(message.id.sft_rev_major, message.id.sft_rev_minor);

                  var component = await VerifyDeviceConfigurationAsync(hardware);

                  hardware.updated_date = DateTime.UtcNow;
                  hardware.is_upload = component.Any(s => s.IsUpload == true);

                  context.hardware.Update(hardware);
                  await context.SaveChangesAsync();

                  // Call Get ip
                  command.GetWebConfigRead(message.id.scp_id, 2);


            }
            else
            {
                  if (!await VerifyHardwareConnection(message.id.scp_id)) return;



                  if (await context.id_report.AnyAsync(x => x.scp_id == message.id.scp_id && x.mac.Equals(UtilitiesHelper.ByteToHexStr(message.id.mac_addr))))
                  {
                        var iDReport = await context.id_report
                            .Where(x => x.scp_id == message.id.scp_id && x.mac.Equals(UtilitiesHelper.ByteToHexStr(message.id.mac_addr)))
                            .OrderBy(x => x.id)
                            .FirstOrDefaultAsync();

                        if (iDReport is null) return;
                        iDReport.device_id = message.id.device_id;
                        iDReport.device_ver = message.id.device_ver;
                        iDReport.software_rev_major = message.id.sft_rev_major;
                        iDReport.software_rev_minor = message.id.sft_rev_minor;
                        iDReport.firmware = UtilitiesHelper.ParseFirmware(message.id.sft_rev_major, message.id.sft_rev_minor);
                        iDReport.serial_number = message.id.serial_number;
                        iDReport.ram_size = message.id.ram_size;
                        iDReport.ram_free = message.id.ram_free;
                        iDReport.e_sec = UtilitiesHelper.UnixToDateTime(message.id.e_sec);
                        iDReport.db_max = message.id.db_max;
                        iDReport.db_active = message.id.db_active;
                        iDReport.dip_switch_powerup = message.id.dip_switch_pwrup;
                        iDReport.dip_switch_current = message.id.dip_switch_current;
                        //iDReport.hardware_id = command.SetScpId(message.id.scp_id, id) ? id : message.id.scp_id;
                        iDReport.firmware_advisory = message.id.firmware_advisory;
                        iDReport.scp_in1 = message.id.scp_in_1;
                        iDReport.scp_in2 = message.id.scp_in_2;
                        iDReport.n_oem_code = message.id.nOemCode;
                        iDReport.config_flag = message.id.config_flags;
                        //iDReport.mac = UtilityHelper.ByteToHexStr(message.id.mac_addr);
                        iDReport.tls_status = message.id.tls_status;
                        iDReport.oper_mode = message.id.oper_mode;
                        iDReport.scp_in3 = message.id.scp_in_3;
                        iDReport.cumulative_bld_cnt = message.id.cumulative_bld_cnt;
                        iDReport.port = "";
                        iDReport.ip = "";
                        context.id_report.Update(iDReport);
                  }
                  else
                  {
                        short id = await helper.GetLowestUnassignedNumberNoLimitAsync<Hardware>(context);
                        Aero.Infrastructure.Data.Entities.IdReport iDReport = new Aero.Infrastructure.Data.Entities.IdReport();
                        iDReport.device_id = message.id.device_id;
                        iDReport.device_ver = message.id.device_ver;
                        iDReport.software_rev_major = message.id.sft_rev_major;
                        iDReport.software_rev_minor = message.id.sft_rev_minor;
                        iDReport.firmware = UtilitiesHelper.ParseFirmware(message.id.sft_rev_major, message.id.sft_rev_minor);
                        iDReport.serial_number = message.id.serial_number;
                        iDReport.ram_size = message.id.ram_size;
                        iDReport.ram_free = message.id.ram_free;
                        iDReport.e_sec = UtilitiesHelper.UnixToDateTime(message.id.e_sec);
                        iDReport.db_max = message.id.db_max;
                        iDReport.db_active = message.id.db_active;
                        iDReport.dip_switch_powerup = message.id.dip_switch_pwrup;
                        iDReport.dip_switch_current = message.id.dip_switch_current;
                        iDReport.scp_id = command.SetScpId(message.id.scp_id, id) ? id : message.id.scp_id;
                        iDReport.firmware_advisory = message.id.firmware_advisory;
                        iDReport.scp_in1 = message.id.scp_in_1;
                        iDReport.scp_in2 = message.id.scp_in_2;
                        iDReport.n_oem_code = message.id.nOemCode;
                        iDReport.config_flag = message.id.config_flags;
                        iDReport.mac = UtilitiesHelper.ByteToHexStr(message.id.mac_addr);
                        iDReport.tls_status = message.id.tls_status;
                        iDReport.oper_mode = message.id.oper_mode;
                        iDReport.scp_in3 = message.id.scp_in_3;
                        iDReport.cumulative_bld_cnt = message.id.cumulative_bld_cnt;
                        iDReport.port = "";
                        iDReport.ip = "";
                        await context.id_report.AddAsync(iDReport);
                  }


                  await context.SaveChangesAsync();


                  command.GetWebConfigRead(message.id.scp_id, 2);


            }

      }

      public async Task<int> DeleteByComponentIdAsync(short component)
      {
            throw new NotImplementedException();
      }

      public async Task<int> UpdateAsync(Hardware newData)
      {
            var en = await context.hardware
            .Where(x => x.mac.Equals(newData.Mac) && x.component_id == newData.ComponentId)
            .OrderBy(x => x.component_id)
            .FirstOrDefaultAsync();

            if(en is null) return 0;

            HardwareMapper.Update(en,newData);

            context.hardware.Update(en);
            return await context.SaveChangesAsync();
      }
}
