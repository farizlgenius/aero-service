using System;
using Aero.Application.DTOs;
using Aero.Application.Interfaces;
using Aero.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Aero.Infrastructure.Repositories;

public class QAlvlRepository(AppDbContext context) : IQAlvlRepository
{
      public async Task<int> CountByLocationIdAndUpdateTimeAsync(short locationId, DateTime sync)
      {
            var res = await context.accesslevel
            .AsNoTracking()
            .Where(x => x.location_id == locationId && x.updated_date > sync)
            .CountAsync();

            return res;
      }

      public async Task<string> GetACRNameByComponentIdAndMacAsync(short component,string mac)
      {
            var res = await context.door.AsNoTracking()
            .OrderBy(x => x.component_id)
            .Where(x => x.component_id == component && x.mac == mac)
            .Select(x => x.name)
            .FirstOrDefaultAsync();

            return res;
      }

      public async Task<IEnumerable<AccessLevelDto>> GetAsync()
      {
            var res = await context.accesslevel
            .AsNoTracking()
            .OrderBy(x => x.component_id)
            .Select(x => new AccessLevelDto
            {
                  // Base
                  Uuid = x.uuid,
                  LocationId = x.location_id,
                  IsActive = x.is_active,

                  // extend_desc
                  Name = x.name,
                  ComponentId = x.component_id,
                  AccessLevelDoorTimeZoneDto = x.accesslevel_door_timezones
                    .Select(x => new AccessLevelDoorTimeZoneDto
                    {
                          Door = new DoorDto
                          {
                                // Base 
                                Uuid = x.door.uuid,
                                ComponentId = x.door.component_id,
                                Mac = x.door.hardware_mac,
                                LocationId = x.door.location_id,
                                IsActive = x.door.is_active,

                                // extend_desc
                                Name = x.door.name,
                                AccessConfig = x.door.access_config,
                                PairDoorNo = x.door.pair_door_no,

                                Readers = new List<ReaderDto>(),
                                ReaderOutConfiguration = x.door.reader_out_config,
                                StrkComponentId = x.door.strike_id,
                                Strk = null,
                                SensorComponentId = x.door.sensor_id,
                                RequestExits = new List<RequestExitDto>(),

                                CardFormat = x.door.card_format,
                                AntiPassbackMode = x.door.antipassback_mode,
                                AntiPassBackIn = (short)(x.door.antipassback_in == null ? 0 : (short)x.door.antipassback_in),
                                AntiPassBackOut = (short)(x.door.antipassback_out == null ? 0 : (short)x.door.antipassback_out),
                                SpareTags = x.door.spare_tag,
                                AccessControlFlags = x.door.access_control_flag,
                                Mode = x.door.mode,
                                ModeDesc = x.door.mode_desc,
                                OfflineMode = x.door.offline_mode,
                                OfflineModeDesc = x.door.offline_mode_desc,
                                DefaultMode = x.door.default_mode,
                                DefaultModeDesc = x.door.default_mode_desc,
                                DefaultLEDMode = x.door.default_led_mode,
                                PreAlarm = x.door.pre_alarm,
                                AntiPassbackDelay = x.door.antipassback_delay,
                                StrkT2 = x.door.strike_t2,
                                DcHeld2 = x.door.dc_held2,
                                StrkFollowPulse = x.door.strike_follow_pulse,
                                StrkFollowDelay = x.door.strike_follow_delay,
                                nExtFeatureType = x.door.n_ext_feature_type,
                                IlPBSio = x.door.i_lpb_sio,
                                IlPBNumber = x.door.i_lpb_number,
                                IlPBLongPress = x.door.i_lpb_long_press,
                                IlPBOutSio = x.door.i_lpb_out_sio,
                                IlPBOutNum = x.door.i_lpb_out_num,
                                DfOfFilterTime = x.door.df_filter_time,
                                MaskForceOpen = x.door.is_force_mask,
                                MaskHeldOpen = x.door.is_held_mask,
                          },
                          TimeZone = new TimeZoneDto
                          {
                                // Base
                                IsActive = x.timezone.is_active,

                                // extend_desc
                                ComponentId = x.timezone.component_id,
                                Name = x.timezone.name,
                                Mode = x.timezone.mode,
                                ActiveTime = x.timezone.active_time,
                                DeactiveTime = x.timezone.deactive_time,
                                Intervals = new List<IntervalDto>()
                          },
                    })
                    .ToList()
            })
            .ToArrayAsync();

            return res;
      }

      public async Task<AccessLevelDto> GetByComponentIdAsync(short componentId)
      {
            var res = await context.accesslevel
            .AsNoTracking()
            .OrderBy(x => x.component_id)
            .Where(x => x.component_id == componentId)
            .Select(x => new AccessLevelDto
            {
                  // Base
                  Uuid = x.uuid,
                  LocationId = x.location_id,
                  IsActive = x.is_active,

                  // extend_desc
                  Name = x.name,
                  ComponentId = x.component_id,
                  AccessLevelDoorTimeZoneDto = x.accesslevel_door_timezones
                    .Select(x => new AccessLevelDoorTimeZoneDto
                    {
                          Door = new DoorDto
                          {
                                // Base 
                                Uuid = x.door.uuid,
                                ComponentId = x.door.component_id,
                                Mac = x.door.hardware_mac,
                                LocationId = x.door.location_id,
                                IsActive = x.door.is_active,

                                // extend_desc
                                Name = x.door.name,
                                AccessConfig = x.door.access_config,
                                PairDoorNo = x.door.pair_door_no,

                                Readers = new List<ReaderDto>(),
                                ReaderOutConfiguration = x.door.reader_out_config,
                                StrkComponentId = x.door.strike_id,
                                Strk = null,
                                SensorComponentId = x.door.sensor_id,
                                RequestExits = new List<RequestExitDto>(),

                                CardFormat = x.door.card_format,
                                AntiPassbackMode = x.door.antipassback_mode,
                                AntiPassBackIn = (short)(x.door.antipassback_in == null ? 0 : (short)x.door.antipassback_in),
                                AntiPassBackOut = (short)(x.door.antipassback_out == null ? 0 : (short)x.door.antipassback_out),
                                SpareTags = x.door.spare_tag,
                                AccessControlFlags = x.door.access_control_flag,
                                Mode = x.door.mode,
                                ModeDesc = x.door.mode_desc,
                                OfflineMode = x.door.offline_mode,
                                OfflineModeDesc = x.door.offline_mode_desc,
                                DefaultMode = x.door.default_mode,
                                DefaultModeDesc = x.door.default_mode_desc,
                                DefaultLEDMode = x.door.default_led_mode,
                                PreAlarm = x.door.pre_alarm,
                                AntiPassbackDelay = x.door.antipassback_delay,
                                StrkT2 = x.door.strike_t2,
                                DcHeld2 = x.door.dc_held2,
                                StrkFollowPulse = x.door.strike_follow_pulse,
                                StrkFollowDelay = x.door.strike_follow_delay,
                                nExtFeatureType = x.door.n_ext_feature_type,
                                IlPBSio = x.door.i_lpb_sio,
                                IlPBNumber = x.door.i_lpb_number,
                                IlPBLongPress = x.door.i_lpb_long_press,
                                IlPBOutSio = x.door.i_lpb_out_sio,
                                IlPBOutNum = x.door.i_lpb_out_num,
                                DfOfFilterTime = x.door.df_filter_time,
                                MaskForceOpen = x.door.is_force_mask,
                                MaskHeldOpen = x.door.is_held_mask,
                          },
                          TimeZone = new TimeZoneDto
                          {
                                // Base
                                IsActive = x.timezone.is_active,

                                // extend_desc
                                ComponentId = x.timezone.component_id,
                                Name = x.timezone.name,
                                Mode = x.timezone.mode,
                                ActiveTime = x.timezone.active_time,
                                DeactiveTime = x.timezone.deactive_time,
                                Intervals = new List<IntervalDto>()
                          },
                    })
                    .ToList()
            })
            .FirstOrDefaultAsync();

            return res;
      }

      public async Task<IEnumerable<AccessLevelDto>> GetByLocationIdAsync(short locationId)
      {
            var res = await context.accesslevel
            .AsNoTracking()
            .OrderBy(x => x.component_id)
            .Where(x => x.location_id == locationId)
            .Select(x => new AccessLevelDto
            {
                  // Base
                  Uuid = x.uuid,
                  LocationId = x.location_id,
                  IsActive = x.is_active,

                  // extend_desc
                  Name = x.name,
                  ComponentId = x.component_id,
                  AccessLevelDoorTimeZoneDto = x.accesslevel_door_timezones
                    .Select(x => new AccessLevelDoorTimeZoneDto
                    {
                          Door = new DoorDto
                          {
                                // Base 
                                Uuid = x.door.uuid,
                                ComponentId = x.door.component_id,
                                Mac = x.door.hardware_mac,
                                LocationId = x.door.location_id,
                                IsActive = x.door.is_active,

                                // extend_desc
                                Name = x.door.name,
                                AccessConfig = x.door.access_config,
                                PairDoorNo = x.door.pair_door_no,

                                Readers = new List<ReaderDto>(),
                                ReaderOutConfiguration = x.door.reader_out_config,
                                StrkComponentId = x.door.strike_id,
                                Strk = null,
                                SensorComponentId = x.door.sensor_id,
                                RequestExits = new List<RequestExitDto>(),

                                CardFormat = x.door.card_format,
                                AntiPassbackMode = x.door.antipassback_mode,
                                AntiPassBackIn = (short)(x.door.antipassback_in == null ? 0 : (short)x.door.antipassback_in),
                                AntiPassBackOut = (short)(x.door.antipassback_out == null ? 0 : (short)x.door.antipassback_out),
                                SpareTags = x.door.spare_tag,
                                AccessControlFlags = x.door.access_control_flag,
                                Mode = x.door.mode,
                                ModeDesc = x.door.mode_desc,
                                OfflineMode = x.door.offline_mode,
                                OfflineModeDesc = x.door.offline_mode_desc,
                                DefaultMode = x.door.default_mode,
                                DefaultModeDesc = x.door.default_mode_desc,
                                DefaultLEDMode = x.door.default_led_mode,
                                PreAlarm = x.door.pre_alarm,
                                AntiPassbackDelay = x.door.antipassback_delay,
                                StrkT2 = x.door.strike_t2,
                                DcHeld2 = x.door.dc_held2,
                                StrkFollowPulse = x.door.strike_follow_pulse,
                                StrkFollowDelay = x.door.strike_follow_delay,
                                nExtFeatureType = x.door.n_ext_feature_type,
                                IlPBSio = x.door.i_lpb_sio,
                                IlPBNumber = x.door.i_lpb_number,
                                IlPBLongPress = x.door.i_lpb_long_press,
                                IlPBOutSio = x.door.i_lpb_out_sio,
                                IlPBOutNum = x.door.i_lpb_out_num,
                                DfOfFilterTime = x.door.df_filter_time,
                                MaskForceOpen = x.door.is_force_mask,
                                MaskHeldOpen = x.door.is_held_mask,
                          },
                          TimeZone = new TimeZoneDto
                          {
                                // Base
                                IsActive = x.timezone.is_active,

                                // extend_desc
                                ComponentId = x.timezone.component_id,
                                Name = x.timezone.name,
                                Mode = x.timezone.mode,
                                ActiveTime = x.timezone.active_time,
                                DeactiveTime = x.timezone.deactive_time,
                                Intervals = new List<IntervalDto>()
                          },
                    })
                    .ToList()
            })
            .ToArrayAsync();

            return res;
      }

      public async Task<short> GetLowestUnassignedNumberAsync(int max)
      {
            if (max <= 0) return -1;

            var query = context.accesslevel
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

      public async Task<string> GetTimezoneNameByComponentIdAsync(short component)
      {
            return await context.timezone.AsNoTracking().Where(x => x.component_id == component).Select(x => x.name).FirstOrDefaultAsync() ?? ""; 
      }

      public async Task<List<string>> GetUniqueMacFromDoorIdAsync(short doorId)
      {
            var res = await context.accesslevel
            .AsNoTracking()
            .Where(x => x.accesslevel_door_timezones.Any(x => x.door_id == doorId))
            .SelectMany(x => x.accesslevel_door_timezones.Select(x => x.door.mac))
            .Distinct()
            .ToListAsync();

            return res;
      }

      public async Task<bool> IsAnyByComponentId(short component)
      {
            return await context.accesslevel.AsNoTracking().AnyAsync(x => x.component_id == component);
      }
}
