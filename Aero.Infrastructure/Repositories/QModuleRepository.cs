using System;
using Aero.Application.DTOs;
using Aero.Application.Interfaces;
using Aero.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Aero.Infrastructure.Repositories;

public sealed class QModuleRepository(AppDbContext context) : IQModuleRepository
{
      public async Task<int> CountByMacAndUpdateTimeAsync(string mac, DateTime sync)
      {
            var res = await context.module
            .AsNoTracking()
            .Where(x => x.hardware_mac.Equals(mac) && x.updated_date > sync)
            .CountAsync();

            return res;
      }

      public async Task<IEnumerable<ModuleDto>> GetAsync()
      {
            var res = await context.module
                .AsNoTracking()
                .Select(d => new ModuleDto
                {
                      // Base 
                      Uuid = d.uuid,
                      ComponentId = d.component_id,
                      Mac = d.hardware_mac,
                      HardwareName = d.hardware.name,
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
                      Readers = d.readers == null ? null : d.readers.Select(x => new ReaderDto
                      {
                            // Base
                            Uuid = x.uuid,
                            ComponentId = x.component_id,
                            Mac = x.module.hardware_mac,
                            HardwareName = x.module.hardware.name,
                            LocationId = x.location_id,
                            IsActive = x.is_active,

                            // extend_desc
                            ModuleId = x.module_id,
                            ReaderNo = x.reader_no,
                            DataFormat = x.data_format,
                            KeypadMode = x.keypad_mode,
                            LedDriveMode = x.led_drive_mode,
                            OsdpFlag = x.osdp_flag,
                            OsdpAddress = x.osdp_address,
                            OsdpBaudrate = x.osdp_baudrate,
                            OsdpDiscover = x.osdp_discover,
                            OsdpSecureChannel = x.osdp_secure_channel,
                            OsdpTracing = x.osdp_tracing,
                      }).ToList(),
                      Sensors = d.sensors == null ? null : d.sensors.Select(x => new SensorDto
                      {

                            // Base 
                            Uuid = x.uuid,
                            ComponentId = x.component_id,
                            Mac = x.module.hardware_mac,
                            HardwareName = x.module.hardware.name,
                            LocationId = x.location_id,
                            IsActive = x.is_active,

                            // extend_desc
                            ModuleId = x.module_id,
                            InputNo = x.input_no,
                            InputMode = x.input_mode,
                            Debounce = x.debounce,
                            HoldTime = x.holdtime,
                            DcHeld = x.dc_held,

                      }).ToList(),
                      Strikes = d.strikes == null ? null : d.strikes.Select(x => new StrikeDto
                      {
                            // Base 
                            Uuid = x.uuid,
                            ComponentId = x.component_id,
                            Mac = x.module.hardware_mac,
                            HardwareName = x.module.hardware.name,
                            LocationId = x.location_id,
                            IsActive = x.is_active,

                            // extend_desc
                            ModuleId = x.module_id,
                            OutputNo = x.output_no,
                            RelayMode = x.relay_mode,
                            OfflineMode = x.offline_mode,
                            StrkMax = x.strike_max,
                            StrkMin = x.strike_min,
                            StrkMode = x.strike_mode,
                      }).ToList(),
                      RequestExits = d.request_exits == null ? null : d.request_exits.Select(x => new RequestExitDto
                      {
                            // Base
                            Uuid = x.uuid,
                            ComponentId = x.component_id,
                            Mac = x.module.hardware_mac,
                            HardwareName = x.module.hardware.name,
                            LocationId = x.location_id,
                            IsActive = x.is_active,

                            // extend_desc
                            ModuleId = x.module_id,
                            InputNo = x.input_no,
                            InputMode = x.input_mode,
                            Debounce = x.debounce,
                            HoldTime = x.holdtime,
                            MaskTimeZone = x.mask_timezone,
                      }).ToList(),
                      MonitorPoints = d.monitor_points == null ? null : d.monitor_points.Select(x => new MonitorPointDto
                      {
                            // Base 
                            Uuid = x.uuid,
                            ComponentId = x.component_id,
                            Mac = x.module.hardware_mac,
                            HardwareName = x.module.hardware.name,
                            LocationId = x.location_id,
                            IsActive = x.is_active,

                            // extend_desc 
                            Name = x.name,
                            ModuleId = x.module_id,
                            InputNo = x.input_no,
                            InputMode = x.input_mode,
                            InputModeDescription = x.input_mode_desc,
                            Debounce = x.debounce,
                            HoldTime = x.holdtime,
                            LogFunction = x.log_function,
                            LogFunctionDescription = x.log_function_desc,
                            MonitorPointMode = x.monitor_point_mode,
                            MonitorPointModeDescription = x.monitor_point_mode_desc,
                            DelayEntry = x.delay_entry,
                            DelayExit = x.delay_exit,
                            IsMask = x.is_mask,

                      }).ToList(),
                      ControlPoints = d.control_points == null ? null : d.control_points.Select(x => new ControlPointDto
                      {
                            // Base
                            Uuid = x.uuid,
                            ComponentId = x.component_id,
                            Mac = x.module.hardware_mac,
                            HardwareName = x.module.hardware.name,
                            LocationId = x.location_id,
                            IsActive = x.is_active,

                            // extend_desc
                            Name = x.name,
                            ModuleId = x.module_id,
                            ModuleDescription = x.module.model_desc,
                            //module_desc = x.module_desc,
                            OutputNo = x.output_no,
                            RelayMode = x.relay_mode,
                            RelayModeDescription = x.relay_mode_desc,
                            OfflineMode = x.offline_mode,
                            OfflineModeDescription = x.offline_mode_desc,
                            DefaultPulse = x.default_pulse,
                      }).ToList(),
                      Address = d.address,
                      Port = d.port,
                      nInput = d.n_input,
                      nOutput = d.n_output,
                      nReader = d.n_reader,
                      Msp1No = d.msp1_no,
                      BaudRate = d.baudrate,
                      nProtocol = d.n_protocol,
                      nDialect = d.n_dialect,
                }).ToArrayAsync();

            return res;
      }

      public Task<ModuleDto> GetByComponentIdAsync(short componentId)
      {
            throw new NotImplementedException();
      }

      public Task<IEnumerable<ModuleDto>> GetByLocationIdAsync(short locationId)
      {
            throw new NotImplementedException();
      }

      public async Task<IEnumerable<ModuleDto>> GetByMacAsync(string mac)
      {
            var res = await context.module
            .AsNoTracking()
            .Where(x => x.hardware_mac.Equals(mac))
            .OrderBy(x => x.component_id)
            .Select(d => new ModuleDto
            {
                  // Base 
                  Uuid = d.uuid,
                  ComponentId = d.component_id,
                  Mac = d.hardware_mac,
                  HardwareName = d.hardware.name,
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
                  Readers = d.readers == null ? null : d.readers.Select(x => new ReaderDto
                  {
                        // Base
                        Uuid = x.uuid,
                        ComponentId = x.component_id,
                        Mac = x.module.hardware_mac,
                        HardwareName = x.module.hardware.name,
                        LocationId = x.location_id,
                        IsActive = x.is_active,

                        // extend_desc
                        ModuleId = x.module_id,
                        ReaderNo = x.reader_no,
                        DataFormat = x.data_format,
                        KeypadMode = x.keypad_mode,
                        LedDriveMode = x.led_drive_mode,
                        OsdpFlag = x.osdp_flag,
                        OsdpAddress = x.osdp_address,
                        OsdpBaudrate = x.osdp_baudrate,
                        OsdpDiscover = x.osdp_discover,
                        OsdpSecureChannel = x.osdp_secure_channel,
                        OsdpTracing = x.osdp_tracing,
                  }).ToList(),
                  Sensors = d.sensors == null ? null : d.sensors.Select(x => new SensorDto
                  {

                        // Base 
                        Uuid = x.uuid,
                        ComponentId = x.component_id,
                        Mac = x.module.hardware_mac,
                        HardwareName = x.module.hardware.name,
                        LocationId = x.location_id,
                        IsActive = x.is_active,

                        // extend_desc
                        ModuleId = x.module_id,
                        InputNo = x.input_no,
                        InputMode = x.input_mode,
                        Debounce = x.debounce,
                        HoldTime = x.holdtime,
                        DcHeld = x.dc_held,

                  }).ToList(),
                  Strikes = d.strikes == null ? null : d.strikes.Select(x => new StrikeDto
                  {
                        // Base 
                        Uuid = x.uuid,
                        ComponentId = x.component_id,
                        Mac = x.module.hardware_mac,
                        HardwareName = x.module.hardware.name,
                        LocationId = x.location_id,
                        IsActive = x.is_active,

                        // extend_desc
                        ModuleId = x.module_id,
                        OutputNo = x.output_no,
                        RelayMode = x.relay_mode,
                        OfflineMode = x.offline_mode,
                        StrkMax = x.strike_max,
                        StrkMin = x.strike_min,
                        StrkMode = x.strike_mode,
                  }).ToList(),
                  RequestExits = d.request_exits == null ? null : d.request_exits.Select(x => new RequestExitDto
                  {
                        // Base
                        Uuid = x.uuid,
                        ComponentId = x.component_id,
                        Mac = x.module.hardware_mac,
                        HardwareName = x.module.hardware.name,
                        LocationId = x.location_id,
                        IsActive = x.is_active,

                        // extend_desc
                        ModuleId = x.module_id,
                        InputNo = x.input_no,
                        InputMode = x.input_mode,
                        Debounce = x.debounce,
                        HoldTime = x.holdtime,
                        MaskTimeZone = x.mask_timezone,
                  }).ToList(),
                  MonitorPoints = d.monitor_points == null ? null : d.monitor_points.Select(x => new MonitorPointDto
                  {
                        // Base 
                        Uuid = x.uuid,
                        ComponentId = x.component_id,
                        Mac = x.module.hardware_mac,
                        HardwareName = x.module.hardware.name,
                        LocationId = x.location_id,
                        IsActive = x.is_active,

                        // extend_desc 
                        Name = x.name,
                        ModuleId = x.module_id,
                        InputNo = x.input_no,
                        InputMode = x.input_mode,
                        InputModeDescription = x.input_mode_desc,
                        Debounce = x.debounce,
                        HoldTime = x.holdtime,
                        LogFunction = x.log_function,
                        LogFunctionDescription = x.log_function_desc,
                        MonitorPointMode = x.monitor_point_mode,
                        MonitorPointModeDescription = x.monitor_point_mode_desc,
                        DelayEntry = x.delay_entry,
                        DelayExit = x.delay_exit,
                        IsMask = x.is_mask,

                  }).ToList(),
                  ControlPoints = d.control_points == null ? null : d.control_points.Select(x => new ControlPointDto
                  {
                        // Base
                        Uuid = x.uuid,
                        ComponentId = x.component_id,
                        Mac = x.module.hardware_mac,
                        HardwareName = x.module.hardware.name,
                        LocationId = x.location_id,
                        IsActive = x.is_active,

                        // extend_desc
                        Name = x.name,
                        ModuleId = x.module_id,
                        ModuleDescription = x.module.model_desc,
                        //module_desc = x.module_desc,
                        OutputNo = x.output_no,
                        RelayMode = x.relay_mode,
                        RelayModeDescription = x.relay_mode_desc,
                        OfflineMode = x.offline_mode,
                        OfflineModeDescription = x.offline_mode_desc,
                        DefaultPulse = x.default_pulse,
                  }).ToList(),
                  Address = d.address,
                  Port = d.port,
                  nInput = d.n_input,
                  nOutput = d.n_output,
                  nReader = d.n_reader,
                  Msp1No = d.msp1_no,
                  BaudRate = d.baudrate,
                  nProtocol = d.n_protocol,
                  nDialect = d.n_dialect,
            })
            .ToArrayAsync();

            return res;
      }

      public Task<short> GetLowestUnassignedNumberAsync(int max)
      {
            throw new NotImplementedException();
      }

      public Task<bool> IsAnyByComponet(short component)
      {
            throw new NotImplementedException();
      }
}
