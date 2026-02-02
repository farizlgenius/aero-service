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

public sealed class HwRepository(AppDbContext context, IScpCommand scp, INotificationPublisher publisher) : IHwRepository
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

      public async Task UpdateIpAddressAsync(int ScpId, string ip)
      {
            var hw = await context.hardware
            .Where(x => x.component_id == (short)ScpId)
            .FirstOrDefaultAsync();

            if (hw is null) return;

            hw.ip = ip;

            context.hardware.Update(hw);
            await context.SaveChangesAsync();
      }

      public async Task UpdatePortAddressAsync(int ScpId, short port)
      {
            var hw = await context.hardware
            .Where(x => x.component_id == (short)ScpId)
            .FirstOrDefaultAsync();

            if (hw is null) return;

            hw.port = ip;

            context.hardware.Update(hw);
            await context.SaveChangesAsync();
      }
}
