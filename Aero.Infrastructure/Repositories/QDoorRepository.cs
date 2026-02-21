using System;
using Aero.Application.DTOs;
using Aero.Application.Interfaces;
using Aero.Domain.Entities;
using Aero.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Aero.Infrastructure.Repositories;

public class QDoorRepository(AppDbContext context) : IQDoorRepository
{
      public async Task<int> CountByMacAndUpdateTimeAsync(string mac, DateTime sync)
      {
            var res = await context.door
            .AsNoTracking()
            .Where(x => x.mac.Equals(mac) && x.updated_date > sync)
            .CountAsync();

            return res;
      }

      public async Task<IEnumerable<Mode>> GetApbModeAsync()
      {
            var dtos = await context.antipassback_mode.Select(x => new Mode 
            {
                Name = x.name,
                Value = x.value,
                Description = x.description

            }).ToArrayAsync();

            return dtos;
      }

      public async Task<IEnumerable<DoorDto>> GetAsync()
      {
            var res = await context.door
            .AsNoTracking()
            .OrderBy(x => x.component_id)
            .Select(x => new DoorDto
            {
                  // Base 
                  
                  ComponentId = x.component_id,
                  Mac = x.hardware_mac,
                  LocationId = x.location_id,
                  IsActive = x.is_active,

                  // extend_desc
                  Name = x.name,
                  AccessConfig = x.access_config,
                  PairDoorNo = x.pair_door_no,

                  // Reader
                  Readers = x.readers == null ? new List<ReaderDto>() : x.readers
                    .Select(x => new ReaderDto
                    {
                          // Base
                          
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
                    })
                    .ToList(),
                  ReaderOutConfiguration = x.reader_out_config,

                  // Strike
                  Strk = x.strike == null ? null : new StrikeDto
                  {
                        // Base 
                        ComponentId = x.strike.component_id,
                        Mac = x.strike.module.hardware_mac,
                        HardwareName = x.strike.module.hardware.name,
                        LocationId = x.location_id,
                        IsActive = x.is_active,

                        // extend_desc
                        ModuleId = x.strike.module_id,
                        OutputNo = x.strike.output_no,
                        RelayMode = x.strike.relay_mode,
                        OfflineMode = x.strike.offline_mode,
                        StrkMax = x.strike.strike_max,
                        StrkMin = x.strike.strike_min,
                        StrkMode = x.strike.strike_mode,
                  },

                  // sensor
                  Sensor = x.sensor == null ? null : new SensorDto
                  {

                        // Base 
                        ComponentId = x.sensor.component_id,
                        Mac = x.sensor.module.hardware_mac,
                        HardwareName = x.sensor.module.hardware.name,
                        LocationId = x.sensor.location_id,
                        IsActive = x.sensor.is_active,

                        // extend_desc
                        ModuleId = x.sensor.module_id,
                        InputNo = x.sensor.input_no,
                        InputMode = x.sensor.input_mode,
                        Debounce = x.sensor.debounce,
                        HoldTime = x.sensor.holdtime,
                        DcHeld = x.sensor.dc_held,

                  },

                  // Request Exit
                  RequestExits = x.request_exits == null ? new List<RequestExitDto>() : x.request_exits
                    .Select(x => new RequestExitDto
                    {
                          // Base
                          
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
                    })
                    .ToList(),


                  CardFormat = x.card_format,
                  AntiPassbackMode = x.antipassback_mode,
                  AntiPassBackIn = (short)(x.antipassback_in == null ? 0 : (short)x.antipassback_in),
                  AntiPassBackOut = (short)(x.antipassback_out == null ? 0 : (short)x.antipassback_out),
                  SpareTags = x.spare_tag,
                  AccessControlFlags = x.access_control_flag,
                  Mode = x.mode,
                  ModeDesc = x.mode_desc,
                  OfflineMode = x.offline_mode,
                  OfflineModeDesc = x.offline_mode_desc,
                  DefaultMode = x.default_mode,
                  DefaultModeDesc = x.default_mode_desc,
                  DefaultLEDMode = x.default_led_mode,
                  PreAlarm = x.pre_alarm,
                  AntiPassbackDelay = x.antipassback_delay,
                  StrkT2 = x.strike_t2,
                  DcHeld2 = x.dc_held2,
                  StrkFollowPulse = x.strike_follow_pulse,
                  StrkFollowDelay = x.strike_follow_delay,
                  nExtFeatureType = x.n_ext_feature_type,
                  IlPBSio = x.i_lpb_sio,
                  IlPBNumber = x.i_lpb_number,
                  IlPBLongPress = x.i_lpb_long_press,
                  IlPBOutSio = x.i_lpb_out_sio,
                  IlPBOutNum = x.i_lpb_out_num,
                  DfOfFilterTime = x.df_filter_time,
                  MaskForceOpen = x.is_force_mask,
                  MaskHeldOpen = x.is_held_mask,

            })
            .ToArrayAsync();

            return res;
      }

      public async Task<IEnumerable<short>> GetAvailableReaderFromMacAndComponentIdAsync(string mac,short component)
      {
            var reader = await context.module
                .AsNoTracking()
                .Where(cp => cp.component_id == component && cp.mac == mac)
                .Select(cp => (short)cp.n_reader)
                .FirstOrDefaultAsync();

            var rdrNos = await context.reader
                .AsNoTracking()
                .Where(cp => cp.module_id == component && cp.module.mac == mac)
                .Select(x => x.reader_no)
                .ToArrayAsync();


            List<short> all = Enumerable.Range(0, reader).Select(i => (short)i).ToList();
            return all.Except(rdrNos).ToList();
      }

      public async Task<DoorDto> GetByComponentIdAsync(short componentId)
      {
            var res = await context.door
            .AsNoTracking()
            .Where(x => x.component_id == componentId)
            .OrderBy(x => x.component_id)
            .Select(x => new DoorDto
            {
                  // Base 
                  
                  ComponentId = x.component_id,
                  Mac = x.hardware_mac,
                  LocationId = x.location_id,
                  IsActive = x.is_active,

                  // extend_desc
                  Name = x.name,
                  AccessConfig = x.access_config,
                  PairDoorNo = x.pair_door_no,

                  // Reader
                  Readers = x.readers == null ? new List<ReaderDto>() : x.readers
                    .Select(x => new ReaderDto
                    {
                          // Base
                          
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
                    })
                    .ToList(),
                  ReaderOutConfiguration = x.reader_out_config,

                  // Strike
                  
                  Strk = x.strike == null ? null : new StrikeDto
                  {
                        // Base 
                        ComponentId = x.strike.component_id,
                        Mac = x.strike.module.hardware_mac,
                        HardwareName = x.strike.module.hardware.name,
                        LocationId = x.location_id,
                        IsActive = x.is_active,

                        // extend_desc
                        ModuleId = x.strike.module_id,
                        OutputNo = x.strike.output_no,
                        RelayMode = x.strike.relay_mode,
                        OfflineMode = x.strike.offline_mode,
                        StrkMax = x.strike.strike_max,
                        StrkMin = x.strike.strike_min,
                        StrkMode = x.strike.strike_mode,
                  },

                  // sensor
                  
                  Sensor = x.sensor == null ? null : new SensorDto
                  {

                        // Base 
                        ComponentId = x.sensor.component_id,
                        Mac = x.sensor.module.hardware_mac,
                        HardwareName = x.sensor.module.hardware.name,
                        LocationId = x.sensor.location_id,
                        IsActive = x.sensor.is_active,

                        // extend_desc
                        ModuleId = x.sensor.module_id,
                        InputNo = x.sensor.input_no,
                        InputMode = x.sensor.input_mode,
                        Debounce = x.sensor.debounce,
                        HoldTime = x.sensor.holdtime,
                        DcHeld = x.sensor.dc_held,

                  },

                  // Request Exit
                  RequestExits = x.request_exits == null ? new List<RequestExitDto>() : x.request_exits
                    .Select(x => new RequestExitDto
                    {
                          // Base
                          
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
                    })
                    .ToList(),


                  CardFormat = x.card_format,
                  AntiPassbackMode = x.antipassback_mode,
                  AntiPassBackIn = (short)(x.antipassback_in == null ? 0 : (short)x.antipassback_in),
                  AntiPassBackOut = (short)(x.antipassback_out == null ? 0 : (short)x.antipassback_out),
                  SpareTags = x.spare_tag,
                  AccessControlFlags = x.access_control_flag,
                  Mode = x.mode,
                  ModeDesc = x.mode_desc,
                  OfflineMode = x.offline_mode,
                  OfflineModeDesc = x.offline_mode_desc,
                  DefaultMode = x.default_mode,
                  DefaultModeDesc = x.default_mode_desc,
                  DefaultLEDMode = x.default_led_mode,
                  PreAlarm = x.pre_alarm,
                  AntiPassbackDelay = x.antipassback_delay,
                  StrkT2 = x.strike_t2,
                  DcHeld2 = x.dc_held2,
                  StrkFollowPulse = x.strike_follow_pulse,
                  StrkFollowDelay = x.strike_follow_delay,
                  nExtFeatureType = x.n_ext_feature_type,
                  IlPBSio = x.i_lpb_sio,
                  IlPBNumber = x.i_lpb_number,
                  IlPBLongPress = x.i_lpb_long_press,
                  IlPBOutSio = x.i_lpb_out_sio,
                  IlPBOutNum = x.i_lpb_out_num,
                  DfOfFilterTime = x.df_filter_time,
                  MaskForceOpen = x.is_force_mask,
                  MaskHeldOpen = x.is_held_mask,

            })
            .FirstOrDefaultAsync();

            return res;
      }

      public async Task<IEnumerable<DoorDto>> GetByLocationIdAsync(short locationId)
      {
            var res = await context.door
.AsNoTracking()
.Where(x => x.location_id == locationId || x.location_id == 1)
.OrderBy(x => x.component_id)
.Select(x => new DoorDto
{
      // Base 
      
      ComponentId = x.component_id,
      Mac = x.hardware_mac,
      LocationId = x.location_id,
      IsActive = x.is_active,

      // extend_desc
      Name = x.name,
      AccessConfig = x.access_config,
      PairDoorNo = x.pair_door_no,

      // Reader
      Readers = x.readers == null ? new List<ReaderDto>() : x.readers
        .Select(x => new ReaderDto
        {
              // Base
              
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
        })
        .ToList(),
      ReaderOutConfiguration = x.reader_out_config,

      // Strike
      
      Strk = x.strike == null ? null : new StrikeDto
      {
            // Base 
            ComponentId = x.strike.component_id,
            Mac = x.strike.module.hardware_mac,
            HardwareName = x.strike.module.hardware.name,
            LocationId = x.location_id,
            IsActive = x.is_active,

            // extend_desc
            ModuleId = x.strike.module_id,
            OutputNo = x.strike.output_no,
            RelayMode = x.strike.relay_mode,
            OfflineMode = x.strike.offline_mode,
            StrkMax = x.strike.strike_max,
            StrkMin = x.strike.strike_min,
            StrkMode = x.strike.strike_mode,
      },

      // sensor
      
      Sensor = x.sensor == null ? null : new SensorDto
      {

            // Base 
            ComponentId = x.sensor.component_id,
            Mac = x.sensor.module.hardware_mac,
            HardwareName = x.sensor.module.hardware.name,
            LocationId = x.sensor.location_id,
            IsActive = x.sensor.is_active,

            // extend_desc
            ModuleId = x.sensor.module_id,
            InputNo = x.sensor.input_no,
            InputMode = x.sensor.input_mode,
            Debounce = x.sensor.debounce,
            HoldTime = x.sensor.holdtime,
            DcHeld = x.sensor.dc_held,

      },

      // Request Exit
      RequestExits = x.request_exits == null ? new List<RequestExitDto>() : x.request_exits
        .Select(x => new RequestExitDto
        {
              // Base
              
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
        })
        .ToList(),


      CardFormat = x.card_format,
      AntiPassbackMode = x.antipassback_mode,
      AntiPassBackIn = (short)(x.antipassback_in == null ? 0 : (short)x.antipassback_in),
      AntiPassBackOut = (short)(x.antipassback_out == null ? 0 : (short)x.antipassback_out),
      SpareTags = x.spare_tag,
      AccessControlFlags = x.access_control_flag,
      Mode = x.mode,
      ModeDesc = x.mode_desc,
      OfflineMode = x.offline_mode,
      OfflineModeDesc = x.offline_mode_desc,
      DefaultMode = x.default_mode,
      DefaultModeDesc = x.default_mode_desc,
      DefaultLEDMode = x.default_led_mode,
      PreAlarm = x.pre_alarm,
      AntiPassbackDelay = x.antipassback_delay,
      StrkT2 = x.strike_t2,
      DcHeld2 = x.dc_held2,
      StrkFollowPulse = x.strike_follow_pulse,
      StrkFollowDelay = x.strike_follow_delay,
      nExtFeatureType = x.n_ext_feature_type,
      IlPBSio = x.i_lpb_sio,
      IlPBNumber = x.i_lpb_number,
      IlPBLongPress = x.i_lpb_long_press,
      IlPBOutSio = x.i_lpb_out_sio,
      IlPBOutNum = x.i_lpb_out_num,
      DfOfFilterTime = x.df_filter_time,
      MaskForceOpen = x.is_force_mask,
      MaskHeldOpen = x.is_held_mask,

})
.ToArrayAsync();

            return res;
      }

      public async Task<IEnumerable<DoorDto>> GetByMacAsync(string mac)
      {
            var res = await context.door
            .AsNoTracking()
            .Where(x => x.mac.Equals(mac))
            .OrderBy(x => x.component_id)
            .Select(x => new DoorDto
            {
                  // Base 
                  
                  ComponentId = x.component_id,
                  Mac = x.hardware_mac,
                  LocationId = x.location_id,
                  IsActive = x.is_active,

                  // extend_desc
                  Name = x.name,
                  AccessConfig = x.access_config,
                  PairDoorNo = x.pair_door_no,

                  // Reader
                  Readers = x.readers == null ? new List<ReaderDto>() : x.readers
                    .Select(x => new ReaderDto
                    {
                          // Base
                          
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
                    })
                    .ToList(),
                  ReaderOutConfiguration = x.reader_out_config,

                  // Strike
                  
                  Strk = x.strike == null ? null : new StrikeDto
                  {
                        // Base 
                        ComponentId = x.strike.component_id,
                        Mac = x.strike.module.hardware_mac,
                        HardwareName = x.strike.module.hardware.name,
                        LocationId = x.location_id,
                        IsActive = x.is_active,

                        // extend_desc
                        ModuleId = x.strike.module_id,
                        OutputNo = x.strike.output_no,
                        RelayMode = x.strike.relay_mode,
                        OfflineMode = x.strike.offline_mode,
                        StrkMax = x.strike.strike_max,
                        StrkMin = x.strike.strike_min,
                        StrkMode = x.strike.strike_mode,
                  },

                  // sensor
                  
                  Sensor = x.sensor == null ? null : new SensorDto
                  {

                        // Base 
                        ComponentId = x.sensor.component_id,
                        Mac = x.sensor.module.hardware_mac,
                        HardwareName = x.sensor.module.hardware.name,
                        LocationId = x.sensor.location_id,
                        IsActive = x.sensor.is_active,

                        // extend_desc
                        ModuleId = x.sensor.module_id,
                        InputNo = x.sensor.input_no,
                        InputMode = x.sensor.input_mode,
                        Debounce = x.sensor.debounce,
                        HoldTime = x.sensor.holdtime,
                        DcHeld = x.sensor.dc_held,

                  },

                  // Request Exit
                  RequestExits = x.request_exits == null ? new List<RequestExitDto>() : x.request_exits
                    .Select(x => new RequestExitDto
                    {
                          // Base
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
                    })
                    .ToList(),


                  CardFormat = x.card_format,
                  AntiPassbackMode = x.antipassback_mode,
                  AntiPassBackIn = (short)(x.antipassback_in == null ? 0 : (short)x.antipassback_in),
                  AntiPassBackOut = (short)(x.antipassback_out == null ? 0 : (short)x.antipassback_out),
                  SpareTags = x.spare_tag,
                  AccessControlFlags = x.access_control_flag,
                  Mode = x.mode,
                  ModeDesc = x.mode_desc,
                  OfflineMode = x.offline_mode,
                  OfflineModeDesc = x.offline_mode_desc,
                  DefaultMode = x.default_mode,
                  DefaultModeDesc = x.default_mode_desc,
                  DefaultLEDMode = x.default_led_mode,
                  PreAlarm = x.pre_alarm,
                  AntiPassbackDelay = x.antipassback_delay,
                  StrkT2 = x.strike_t2,
                  DcHeld2 = x.dc_held2,
                  StrkFollowPulse = x.strike_follow_pulse,
                  StrkFollowDelay = x.strike_follow_delay,
                  nExtFeatureType = x.n_ext_feature_type,
                  IlPBSio = x.i_lpb_sio,
                  IlPBNumber = x.i_lpb_number,
                  IlPBLongPress = x.i_lpb_long_press,
                  IlPBOutSio = x.i_lpb_out_sio,
                  IlPBOutNum = x.i_lpb_out_num,
                  DfOfFilterTime = x.df_filter_time,
                  MaskForceOpen = x.is_force_mask,
                  MaskHeldOpen = x.is_held_mask,

            })
            .ToArrayAsync();

            return res;
      }

      public async Task<IEnumerable<Mode>> GetDoorModeAsync()
      {
            var dtos = await context.door_mode.Select(x => new Mode 
            {
                Name = x.name,
                Value = x.value,
                Description = x.description

            }).ToArrayAsync();

            return dtos;
      }

      public async Task<short> GetLowestUnassignedNumberAsync(int max,string mac)
      {
            if (max <= 0) return -1;

            var query = context.door
                .AsNoTracking()
                .Select(x => x.component_id);

            // Handle empty table case quickly
            var hasAny = await query.AnyAsync();
            if (!hasAny)
                return 1; // start at 1 if table is empty

            // Load all numbers into memory (only the column, so it's lightweight)
            var numbers = await query.Distinct().OrderBy(x => x).ToListAsync();

            short expected = 1;
            foreach (var num in numbers)
            {
                if (num != expected)
                    return expected; // found the lowest missing number
                expected++;
            }

            // If none missing in sequence, return next number
            if (expected > max) return -1;
            return expected;
      }

      public async Task<short> GetLowestUnassignedNumberByMacAsync(string mac,int max)
      {
             if (max <= 0) return -1;

            var query = context.door
                .AsNoTracking()
                .Where(x => x.mac == mac)
                .Select(x => x.component_id);

            // Handle empty table case quickly
            var hasAny = await query.AnyAsync();
            if (!hasAny)
                return 0; // start at 0 if table is empty

            // Load all numbers into memory (only the column, so it's lightweight)
            var numbers = await query.Distinct().OrderBy(x => x).ToListAsync();

            short expected = 1;
            foreach (var num in numbers)
            {
                if (num != expected)
                    return expected; // found the lowest missing number
                expected++;
            }

            // If none missing in sequence, return next number
            if (expected > max) return -1;
            return expected;
      }

      public async Task<short> GetLowestUnassignedReaderNumberNoLimitAsync()
      {
             var query = context.reader
                .AsNoTracking()
                .Select(x => x.component_id);

            // Handle empty table case quickly
            var hasAny = await query.AnyAsync();
            if (!hasAny)
                return 1; // start at 1 if table is empty

            // Load all numbers into memory (only the column, so it's lightweight)
            var numbers = await query.Distinct().OrderBy(x => x).ToListAsync();

            short expected = 1;
            foreach (var num in numbers)
            {
                if (num != expected)
                    return expected; // found the lowest missing number
                expected++;
            }

            // If none missing in sequence, return next number
            return expected;
      }

      public async Task<short> GetLowestUnassignedSensorNumberNoLimitAsync()
      {
             var query = context.sensor
                .AsNoTracking()
                .Select(x => x.component_id);

            // Handle empty table case quickly
            var hasAny = await query.AnyAsync();
            if (!hasAny)
                return 1; // start at 1 if table is empty

            // Load all numbers into memory (only the column, so it's lightweight)
            var numbers = await query.Distinct().OrderBy(x => x).ToListAsync();

            short expected = 1;
            foreach (var num in numbers)
            {
                if (num != expected)
                    return expected; // found the lowest missing number
                expected++;
            }

            // If none missing in sequence, return next number
            return expected;
      }

      public async Task<short> GetLowestUnassignedStrikeNumberNoLimitAsync()
      {
             var query = context.strike
                .AsNoTracking()
                .Select(x => x.component_id);

            // Handle empty table case quickly
            var hasAny = await query.AnyAsync();
            if (!hasAny)
                return 1; // start at 1 if table is empty

            // Load all numbers into memory (only the column, so it's lightweight)
            var numbers = await query.Distinct().OrderBy(x => x).ToListAsync();

            short expected = 1;
            foreach (var num in numbers)
            {
                if (num != expected)
                    return expected; // found the lowest missing number
                expected++;
            }

            // If none missing in sequence, return next number
            return expected;
      }

      public async Task<short> GetLowestUnassignedRexNumberAsync()
      {
            var query = context.request_exit
                .AsNoTracking()
                .Select(x => x.component_id);

            // Handle empty table case quickly
            var hasAny = await query.AnyAsync();
            if (!hasAny)
                  return 1; // start at 1 if table is empty

            // Load all numbers into memory (only the column, so it's lightweight)
            var numbers = await query.Distinct().OrderBy(x => x).ToListAsync();

            short expected = 1;
            foreach (var num in numbers)
            {
                  if (num != expected)
                        return expected; // found the lowest missing number
                  expected++;
            }

            // If none missing in sequence, return next number
            return expected;
      }

      public async Task<IEnumerable<Mode>> GetReaderModeAsync()
      {
             var dtos = await context.reader_configuration_mode.Select(x => new Mode 
            {
                Name = x.name,
                Value = x.value,
                Description = x.description

            }).ToArrayAsync();

            return dtos;
      }

      public async Task<IEnumerable<Mode>> GetReaderOutModeAsync()
      {
            var res = await context.reader_out_configuration
                .Select(x => new Mode
                {
                    Name = x.name,
                    Value = x.value,
                    Description = x.description

                }).ToArrayAsync();

            return res;
      }

      public async Task<IEnumerable<Mode>> GetStrikeModeAsync()
      {
             var dtos = await context.strike_mode.Select(x => new Mode 
            {
                Name = x.name,
                Value = x.value,
                Description = x.description

            }).ToArrayAsync();

            return dtos;
      }

      public async Task<bool> IsAnyByComponentId(short component)
      {
        return await context.door.AnyAsync(x => x.component_id == component);
      }

      public async Task<IEnumerable<Mode>> GetDoorAccessControlFlagAsync()
      {
            var dtos = await context.door_access_control_flag
                .Select(x => new Mode 
                {
                    Name = x.name,
                    Value = x.value,
                    Description = x.description
                })
                .ToArrayAsync();

            return dtos;
      }

      public async Task<IEnumerable<Mode>> GetDoorSpareFlagAsync()
      {
             var dtos = await context.door_spare_flag
                .Select(x => new Mode 
                {
                    Name = x.name,
                    Value = x.value,
                    Description = x.description
                })
                .ToArrayAsync();

            return dtos;
      }

      public async Task<IEnumerable<Mode>> GetOsdpBaudrateAsync()
      {
             var dtos = await context.osdp_baudrate.Select(x => new Mode
            {
                Name = x.name,
                Value = x.value,
                Description = x.description,
            }).ToArrayAsync();

            return dtos;
      }

      public async Task<IEnumerable<Mode>> GetOsdpAddressAsync()
      {
             var dtos = await context.osdp_address.Select(x => new Mode
            {
                Name = x.name,
                Value = x.value,
                Description = x.description,
            }).ToArrayAsync();

            return dtos;
      }

    public async Task<Pagination<DoorDto>> GetPaginationAsync(PaginationParamsWithFilter param, short location)
    {

        var query = context.door.AsNoTracking().AsQueryable();


        if (!string.IsNullOrWhiteSpace(param.Search))
        {
            if (!string.IsNullOrWhiteSpace(param.Search))
            {
                var search = param.Search.Trim();

                if (context.Database.IsNpgsql())
                {
                    var pattern = $"%{search}%";

                    query = query.Where(x =>
                        EF.Functions.ILike(x.name, pattern) ||
                        EF.Functions.ILike(x.mac, pattern) ||
                        EF.Functions.ILike(x.mac,pattern)

                    );
                }
                else // SQL Server
                {
                    query = query.Where(x =>
                        x.name.Contains(search) ||
                        x.mac.Contains(search) ||
                        x.mac.Contains(search) 
                    );
                }
            }
        }

        query = query.Where(x => x.location_id == location || x.location_id == 1);

        if (param.StartDate != null)
        {
            var startUtc = DateTime.SpecifyKind(param.StartDate.Value, DateTimeKind.Utc);
            query = query.Where(x => x.created_date >= startUtc);
        }

        if (param.EndDate != null)
        {
            var endUtc = DateTime.SpecifyKind(param.EndDate.Value, DateTimeKind.Utc);
            query = query.Where(x => x.created_date <= endUtc);
        }

        var count = await query.CountAsync();


        var data = await query
            .AsNoTracking()
            .OrderByDescending(t => t.created_date)
            .Skip((param.PageNumber - 1) * param.PageSize)
            .Take(param.PageSize)
             .Select(x => new DoorDto
             {
                 // Base 

                 ComponentId = x.component_id,
                 Mac = x.mac,
                 LocationId = x.location_id,
                 IsActive = x.is_active,

                 // extend_desc
                 Name = x.name,
                 AccessConfig = x.access_config,
                 PairDoorNo = x.pair_door_no,

                 // Reader
                 Readers = x.readers == null ? new List<ReaderDto>() : x.readers
                    .Select(x => new ReaderDto
                    {
                        // Base

                        ComponentId = x.component_id,
                        Mac = x.module.mac,
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
                    })
                    .ToList(),
                 ReaderOutConfiguration = x.reader_out_config,

                 // Strike
                 
                 Strk = x.strike == null ? null : new StrikeDto
                 {
                     // Base 
                     ComponentId = x.strike.component_id,
                     Mac = x.strike.module.mac,
                     HardwareName = x.strike.module.hardware.name,
                     LocationId = x.location_id,
                     IsActive = x.is_active,

                     // extend_desc
                     ModuleId = x.strike.module_id,
                     OutputNo = x.strike.output_no,
                     RelayMode = x.strike.relay_mode,
                     OfflineMode = x.strike.offline_mode,
                     StrkMax = x.strike.strike_max,
                     StrkMin = x.strike.strike_min,
                     StrkMode = x.strike.strike_mode,
                 },

                 // sensor
                 
                 Sensor = x.sensor == null ? null : new SensorDto
                 {

                     // Base 
                     ComponentId = x.sensor.component_id,
                     Mac = x.sensor.module.mac,
                     HardwareName = x.sensor.module.hardware.name,
                     LocationId = x.sensor.location_id,
                     IsActive = x.sensor.is_active,

                     // extend_desc
                     ModuleId = x.sensor.module_id,
                     InputNo = x.sensor.input_no,
                     InputMode = x.sensor.input_mode,
                     Debounce = x.sensor.debounce,
                     HoldTime = x.sensor.holdtime,
                     DcHeld = x.sensor.dc_held,

                 },

                 // Request Exit
                 RequestExits = x.request_exits == null ? new List<RequestExitDto>() : x.request_exits
                    .Select(x => new RequestExitDto
                    {
                        // Base

                        ComponentId = x.component_id,
                        Mac = x.module.mac,
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
                    })
                    .ToList(),


                 CardFormat = x.card_format,
                 AntiPassbackMode = x.antipassback_mode,
                 AntiPassBackIn = (short)(x.antipassback_in == null ? 0 : (short)x.antipassback_in),
                 AntiPassBackOut = (short)(x.antipassback_out == null ? 0 : (short)x.antipassback_out),
                 SpareTags = x.spare_tag,
                 AccessControlFlags = x.access_control_flag,
                 Mode = x.mode,
                 ModeDesc = x.mode_desc,
                 OfflineMode = x.offline_mode,
                 OfflineModeDesc = x.offline_mode_desc,
                 DefaultMode = x.default_mode,
                 DefaultModeDesc = x.default_mode_desc,
                 DefaultLEDMode = x.default_led_mode,
                 PreAlarm = x.pre_alarm,
                 AntiPassbackDelay = x.antipassback_delay,
                 StrkT2 = x.strike_t2,
                 DcHeld2 = x.dc_held2,
                 StrkFollowPulse = x.strike_follow_pulse,
                 StrkFollowDelay = x.strike_follow_delay,
                 nExtFeatureType = x.n_ext_feature_type,
                 IlPBSio = x.i_lpb_sio,
                 IlPBNumber = x.i_lpb_number,
                 IlPBLongPress = x.i_lpb_long_press,
                 IlPBOutSio = x.i_lpb_out_sio,
                 IlPBOutNum = x.i_lpb_out_num,
                 DfOfFilterTime = x.df_filter_time,
                 MaskForceOpen = x.is_force_mask,
                 MaskHeldOpen = x.is_held_mask,

             })
            .ToArrayAsync();


        return new Pagination<DoorDto>
        {
            Data = data,
            Page = new PaginationData
            {
                TotalCount = count,
                PageNumber = param.PageNumber,
                PageSize = param.PageSize,
                TotalPage = (int)Math.Ceiling(count / (double)param.PageSize)
            }
        };
    }
}
