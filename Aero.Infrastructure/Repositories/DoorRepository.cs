using Aero.Application.DTOs;
using Aero.Domain.Entities;
using Aero.Domain.Interface;
using Aero.Infrastructure.Persistences;
using Aero.Infrastructure.Mapper;
using Microsoft.EntityFrameworkCore;
using System;
using Aero.Application.Interface;
using Aero.Domain.Enums;

namespace Aero.Infrastructure.Repositories;

public class DoorRepository(AppDbContext context) : IDoorRepository
{
    public async Task<int> AddAsync(Door data)
    {
        var com = new Persistences.Entities.AccessLevelComponent(2, data.DeviceId, data.Id, data.DriverId, 1);
        // Add All Door to Access Level All

        var en = new Persistences.Entities.Door(data.DriverId, data.Name, data.AccessConfig, data.Direction, data.PairDoorNo, data.DeviceId, data.LocationId, data.Readers, data.ReaderOutConfiguration,
        data.Strk, data.Sensor, data.RequestExits, data.CardFormat, data.AntiPassbackMode, data.AntiPassBackIn, data.AreaInId, data.AntiPassBackOut,
        data.AreaOutId, data.SpareTags, data.AccessControlFlags, data.Mode, data.ModeDesc, data.OfflineMode, data.OfflineModeDesc, data.DefaultMode, data.DefaultModeDesc, data.DefaultLEDMode, data.PreAlarm,
        data.AntiPassbackDelay, data.StrkT2, data.DcHeld2, data.StrkFollowPulse, data.StrkFollowDelay, data.nExtFeatureType, data.IlPBSio, data.IlPBNumber, data.IlPBLongPress, data.IlPBOutSio,
        data.IlPBOutNum, data.DfOfFilterTime, data.MaskHeldOpen, data.MaskForceOpen);
        await context.door.AddAsync(en);
        var rec = await context.SaveChangesAsync();
        if (rec <= 0) return -1;
        return en.id;
    }

    public async Task<int> ChangeDoorModeAsync(int deviceid, short driverid, short mode)
    {
        var en = await context.door
        .Where(x => x.device_id == deviceid && x.driver_id == driverid)
        .OrderBy(x => x.id)
        .FirstOrDefaultAsync();

        if (en is null) return 0;

        en.mode = mode;
        en.mode_detail = await context.door_mode
            .AsNoTracking()
            .Where(x => x.value == en.mode)
            .Select(x => x.name)
            .FirstOrDefaultAsync() ?? "";
        context.Update(en);
        return await context.SaveChangesAsync();
    }

    public async Task<int> DeleteByIdAsync(int Id)
    {
        var en = await context.door
        .Where(x => x.id == Id)
        .OrderBy(x => x.id)
        .FirstOrDefaultAsync();

        if (en is null) return 0;

        context.door.Remove(en);
        return await context.SaveChangesAsync();
    }

    public async Task<int> UpdateAsync(Door data)
    {
        var en = await context.door
        .Include(x => x.readers)
        .Include(x => x.sensor)
        .Include(x => x.request_exits)
        .Include(x => x.strike)
        .Where(x => x.id == data.Id)
        .OrderBy(x => x.id)
        .FirstOrDefaultAsync();

        if (en is null) return 0;

        // Delete old component
        context.reader.RemoveRange(en.readers);
        context.sensor.Remove(en.sensor);
        context.strike.Remove(en.strike);
        if (en.request_exits != null) context.request_exit.RemoveRange(en.request_exits);

        en.Update(data);

        return await context.SaveChangesAsync();

    }

    public async Task<int> CountByDeviceIdAndUpdateTimeAsync(int device, DateTime sync)
    {
        var res = await context.door
        .AsNoTracking()
        .Where(x => x.device_id == device && x.updated_date > sync)
        .CountAsync();

        return res;
    }

    public async Task<IEnumerable<ModeDto>> GetApbModeAsync()
    {
        var dtos = await context.antipassback_mode
        .AsNoTracking()
        .Select(x => new ModeDto(x.name, x.value, x.description))
        .ToArrayAsync();

        return dtos;
    }

    public async Task<IEnumerable<DoorDto>> GetAsync()
    {
        var data = await context.door
        .AsNoTracking()
        .OrderBy(x => x.id)
        .Select(d => new
        {
            d.id,
            d.device_id,
            d.driver_id,
            d.name,
            d.access_config,
            d.pair_door_no,
            d.direction,
            readers = d.readers.Select(
            r => new
            {
                r.device_id,
                r.door_id,
                r.module_id,
                r.reader_no,
                r.data_format,
                r.keypad_mode,
                r.led_drive_mode,
                r.osdp_flag,
                r.osdp_baudrate,
                r.osdp_discover,
                r.osdp_tracing,
                r.osdp_address,
                r.osdp_secure_channel,
                r.location_id,
                r.is_active
            }
            ),
            d.reader_out_config,
            strike = d.strike == null ? null : new StrikeDto(d.strike.device_id, d.strike.door_id, (short)d.strike.module_id, d.strike.output_no, d.strike.relay_mode, d.offline_mode, d.strike.strike_max, d.strike.strike_min, d.strike.strike_mode, d.strike.location_id, d.strike.is_active),
            sensor = d.sensor == null ? null : new SensorDto(d.sensor.device_id, d.sensor.door_id, (short)d.sensor.module_id, d.sensor.input_no, d.sensor.input_mode, d.sensor.debounce, d.sensor.holdtime, d.sensor.dc_held, d.sensor.location_id, d.sensor.is_active),
            rexs = d.request_exits == null ? null : d.request_exits.Select(
                rx => new
                {
                    rx.device_id,
                    rx.door_id,
                    rx.module_id,
                    rx.input_no,
                    rx.input_mode,
                    rx.debounce,
                    rx.holdtime,
                    rx.mask_timezone,
                    rx.location_id,
                    rx.is_active

                }
            ),
            d.card_format,
            d.antipassback_mode,
            d.antipassback_in,
            d.area_in_id,
            d.antipassback_out,
            d.area_out_id,
            d.spare_tag,
            d.access_control_flag,
            d.mode,
            d.mode_detail,
            d.offline_mode,
            d.offline_mode_detail,
            d.default_mode,
            d.default_mode_detail,
            d.default_led_mode,
            d.pre_alarm,
            d.antipassback_delay,
            d.strike_t2,
            d.dc_held2,
            d.strike_follow_pulse,
            d.strike_follow_delay,
            d.n_ext_feature_type,
            d.i_lpb_sio,
            d.i_lpb_number,
            d.i_lpb_long_press,
            d.i_lpb_out_sio,
            d.i_lpb_out_num,
            d.df_filter_time,
            d.is_held_mask,
            d.is_force_mask,
            d.location_id,
            d.is_active
        }
        )
        .ToArrayAsync();

        var res = data.Select(d => new DoorDto
        (
            d.id,
            d.device_id,
            d.driver_id,
            d.name,
            d.access_config,
            d.pair_door_no,
            (int)d.direction,
            d.readers.Select(r => new ReaderDto(r.device_id, r.module_id, r.door_id, r.reader_no, r.data_format, r.keypad_mode, r.led_drive_mode, r.osdp_flag, r.osdp_baudrate, r.osdp_discover, r.osdp_tracing, r.osdp_address, r.osdp_secure_channel, r.location_id, r.is_active)).ToList(),
            d.reader_out_config,
            d.strike,
            d.sensor,
            d.rexs is null ? new List<RequestExitDto>() : d.rexs.Select(rx => new RequestExitDto(rx.device_id,rx.module_id,rx.door_id, rx.input_no, rx.input_mode, rx.debounce, rx.holdtime, rx.mask_timezone, rx.location_id, rx.is_active)).ToList(),
            d.card_format, d.antipassback_mode, d.antipassback_in, d.area_in_id, d.antipassback_out, d.area_out_id, d.spare_tag, d.access_control_flag, d.mode, d.mode_detail, d.offline_mode, d.offline_mode_detail,
        d.default_mode, d.default_mode_detail, d.default_led_mode, d.pre_alarm, d.antipassback_delay, d.strike_t2, d.dc_held2, d.strike_follow_pulse, d.strike_follow_delay,
        d.n_ext_feature_type, d.i_lpb_sio, d.i_lpb_number, d.i_lpb_long_press, d.i_lpb_out_sio, d.i_lpb_out_num,
        d.df_filter_time, d.is_held_mask, d.is_force_mask, d.location_id, d.is_active
        )).ToList();

        return res;
    }

    public async Task<IEnumerable<short>> GetAvailableReaderFromDeviceIdAndDriverIdAsync(int device, int driver)
    {
        var reader = await context.module
            .AsNoTracking()
            .Where(cp => cp.driver_id == driver && cp.device_id == device)
            .Select(cp => (short)cp.n_reader)
            .FirstOrDefaultAsync();

        var rdrNos = await context.reader
            .AsNoTracking()
            .Where(cp => cp.module_id == driver && cp.module.device_id == device)
            .Select(x => x.reader_no)
            .ToArrayAsync();


        List<short> all = Enumerable.Range(0, reader).Select(i => (short)i).ToList();
        return all.Except(rdrNos).ToList();
    }

    public async Task<DoorDto> GetByIdAsync(int id)
    {
        var d = await context.door
        .AsNoTracking()
        .Where(x => x.id == id)
        .OrderBy(x => x.id)
        .Select(d => new
        {
            d.id,
            d.device_id,
            d.driver_id,
            d.name,
            d.access_config,
            d.pair_door_no,
            d.direction,
            readers = d.readers.Select(
            r => new
            {
                r.device_id,
                r.door_id,
                r.module_id,
                r.reader_no,
                r.data_format,
                r.keypad_mode,
                r.led_drive_mode,
                r.osdp_flag,
                r.osdp_baudrate,
                r.osdp_discover,
                r.osdp_tracing,
                r.osdp_address,
                r.osdp_secure_channel,
                r.location_id,
                r.is_active
            }
            ),
            d.reader_out_config,
            strike = d.strike == null ? null : new StrikeDto(d.strike.device_id, d.strike.door_id, (short)d.strike.module_id, d.strike.output_no, d.strike.relay_mode, d.offline_mode, d.strike.strike_max, d.strike.strike_min, d.strike.strike_mode, d.strike.location_id, d.strike.is_active),
            sensor = d.sensor == null ? null : new SensorDto(d.sensor.device_id, d.sensor.door_id, (short)d.sensor.module_id, d.sensor.input_no, d.sensor.input_mode, d.sensor.debounce, d.sensor.holdtime, d.sensor.dc_held, d.sensor.location_id, d.sensor.is_active),
            rexs = d.request_exits == null ? null : d.request_exits.Select(
                rx => new
                {
                    rx.device_id,
                    rx.door_id,
                    rx.module_id,
                    rx.input_no,
                    rx.input_mode,
                    rx.debounce,
                    rx.holdtime,
                    rx.mask_timezone,
                    rx.location_id,
                    rx.is_active

                }
            ),
            d.card_format,
            d.antipassback_mode,
            d.antipassback_in,
            d.area_in_id,
            d.antipassback_out,
            d.area_out_id,
            d.spare_tag,
            d.access_control_flag,
            d.mode,
            d.mode_detail,
            d.offline_mode,
            d.offline_mode_detail,
            d.default_mode,
            d.default_mode_detail,
            d.default_led_mode,
            d.pre_alarm,
            d.antipassback_delay,
            d.strike_t2,
            d.dc_held2,
            d.strike_follow_pulse,
            d.strike_follow_delay,
            d.n_ext_feature_type,
            d.i_lpb_sio,
            d.i_lpb_number,
            d.i_lpb_long_press,
            d.i_lpb_out_sio,
            d.i_lpb_out_num,
            d.df_filter_time,
            d.is_held_mask,
            d.is_force_mask,
            d.location_id,
            d.is_active
        }
        )
        .FirstOrDefaultAsync();

        if (d is null) return null;

        var res = new DoorDto
        (
            d.id,
            d.device_id,
            d.driver_id,
            d.name,
            d.access_config,
            d.pair_door_no,
            (int)d.direction,
            d.readers.Select(r => new ReaderDto(r.device_id,r.module_id, r.door_id,r.reader_no, r.data_format, r.keypad_mode, r.led_drive_mode, r.osdp_flag, r.osdp_baudrate, r.osdp_discover, r.osdp_tracing, r.osdp_address, r.osdp_secure_channel, r.location_id, r.is_active)).ToList(),
            d.reader_out_config,
            d.strike,
            d.sensor,
            d.rexs is null ? new List<RequestExitDto>() : d.rexs.Select(rx => new RequestExitDto(rx.device_id,rx.module_id,rx.door_id, rx.input_no, rx.input_mode, rx.debounce, rx.holdtime, rx.mask_timezone, rx.location_id, rx.is_active)).ToList(),
            d.card_format, d.antipassback_mode, d.antipassback_in, d.area_in_id, d.antipassback_out, d.area_out_id, d.spare_tag, d.access_control_flag, d.mode, d.mode_detail, d.offline_mode, d.offline_mode_detail,
        d.default_mode, d.default_mode_detail, d.default_led_mode, d.pre_alarm, d.antipassback_delay, d.strike_t2, d.dc_held2, d.strike_follow_pulse, d.strike_follow_delay,
        d.n_ext_feature_type, d.i_lpb_sio, d.i_lpb_number, d.i_lpb_long_press, d.i_lpb_out_sio, d.i_lpb_out_num,
        d.df_filter_time, d.is_held_mask, d.is_force_mask, d.location_id, d.is_active
        );

        return res;
    }

    public async Task<IEnumerable<DoorDto>> GetByLocationIdAsync(int location)
    {
        var data = await context.door
            .AsNoTracking()
            .Where(x => x.location_id == location || x.location_id == 1)
            .OrderBy(x => x.id)
            .Select(d => new
        {
            d.id,
            d.device_id,
            d.driver_id,
            d.name,
            d.access_config,
            d.pair_door_no,
            d.direction,
            readers = d.readers.Select(
            r => new
            {
                r.device_id,
                r.door_id,
                r.module_id,
                r.reader_no,
                r.data_format,
                r.keypad_mode,
                r.led_drive_mode,
                r.osdp_flag,
                r.osdp_baudrate,
                r.osdp_discover,
                r.osdp_tracing,
                r.osdp_address,
                r.osdp_secure_channel,
                r.location_id,
                r.is_active
            }
            ),
            d.reader_out_config,
            strike = d.strike == null ? null : new StrikeDto(d.strike.device_id, d.strike.door_id, (short)d.strike.module_id, d.strike.output_no, d.strike.relay_mode, d.offline_mode, d.strike.strike_max, d.strike.strike_min, d.strike.strike_mode, d.strike.location_id, d.strike.is_active),
            sensor = d.sensor == null ? null : new SensorDto(d.sensor.device_id, d.sensor.door_id, (short)d.sensor.module_id, d.sensor.input_no, d.sensor.input_mode, d.sensor.debounce, d.sensor.holdtime, d.sensor.dc_held, d.sensor.location_id, d.sensor.is_active),
            rexs = d.request_exits == null ? null : d.request_exits.Select(
                rx => new
                {
                    rx.device_id,
                    rx.door_id,
                    rx.module_id,
                    rx.input_no,
                    rx.input_mode,
                    rx.debounce,
                    rx.holdtime,
                    rx.mask_timezone,
                    rx.location_id,
                    rx.is_active

                }
            ),
            d.card_format,
            d.antipassback_mode,
            d.antipassback_in,
            d.area_in_id,
            d.antipassback_out,
            d.area_out_id,
            d.spare_tag,
            d.access_control_flag,
            d.mode,
            d.mode_detail,
            d.offline_mode,
            d.offline_mode_detail,
            d.default_mode,
            d.default_mode_detail,
            d.default_led_mode,
            d.pre_alarm,
            d.antipassback_delay,
            d.strike_t2,
            d.dc_held2,
            d.strike_follow_pulse,
            d.strike_follow_delay,
            d.n_ext_feature_type,
            d.i_lpb_sio,
            d.i_lpb_number,
            d.i_lpb_long_press,
            d.i_lpb_out_sio,
            d.i_lpb_out_num,
            d.df_filter_time,
            d.is_held_mask,
            d.is_force_mask,
            d.location_id,
            d.is_active
        }
        )
            .ToArrayAsync();

        var res = data.Select(d => new DoorDto
    (
        d.id,
        d.device_id,
        d.driver_id,
        d.name,
        d.access_config,
        d.pair_door_no,
        (int)d.direction,
        d.readers.Select(r => new ReaderDto(r.device_id,r.module_id,r.door_id,r.reader_no, r.data_format, r.keypad_mode, r.led_drive_mode, r.osdp_flag, r.osdp_baudrate, r.osdp_discover, r.osdp_tracing, r.osdp_address, r.osdp_secure_channel, r.location_id, r.is_active)).ToList(),
        d.reader_out_config,
        d.strike,
        d.sensor,
        d.rexs is null ? new List<RequestExitDto>() : d.rexs.Select(rx => new RequestExitDto(rx.device_id,rx.module_id,rx.door_id, rx.input_no, rx.input_mode, rx.debounce, rx.holdtime, rx.mask_timezone, rx.location_id, rx.is_active)).ToList(),
        d.card_format, d.antipassback_mode, d.antipassback_in, d.area_in_id, d.antipassback_out, d.area_out_id, d.spare_tag, d.access_control_flag, d.mode, d.mode_detail, d.offline_mode, d.offline_mode_detail,
    d.default_mode, d.default_mode_detail, d.default_led_mode, d.pre_alarm, d.antipassback_delay, d.strike_t2, d.dc_held2, d.strike_follow_pulse, d.strike_follow_delay,
    d.n_ext_feature_type, d.i_lpb_sio, d.i_lpb_number, d.i_lpb_long_press, d.i_lpb_out_sio, d.i_lpb_out_num,
    d.df_filter_time, d.is_held_mask, d.is_force_mask, d.location_id, d.is_active
    )).ToList();

        return res;
    }

    public async Task<IEnumerable<DoorDto>> GetByDeviceIdAsync(int device)
    {
        var data = await context.door
        .AsNoTracking()
        .Where(x => x.device_id == device)
        .OrderBy(x => x.id)
         .Select(d => new
        {
            d.id,
            d.device_id,
            d.driver_id,
            d.name,
            d.access_config,
            d.pair_door_no,
            d.direction,
            readers = d.readers.Select(
            r => new
            {
                r.device_id,
                r.door_id,
                r.module_id,
                r.reader_no,
                r.data_format,
                r.keypad_mode,
                r.led_drive_mode,
                r.osdp_flag,
                r.osdp_baudrate,
                r.osdp_discover,
                r.osdp_tracing,
                r.osdp_address,
                r.osdp_secure_channel,
                r.location_id,
                r.is_active
            }
            ),
            d.reader_out_config,
            strike = d.strike == null ? null : new StrikeDto(d.strike.device_id, d.strike.door_id, (short)d.strike.module_id, d.strike.output_no, d.strike.relay_mode, d.offline_mode, d.strike.strike_max, d.strike.strike_min, d.strike.strike_mode, d.strike.location_id, d.strike.is_active),
            sensor = d.sensor == null ? null : new SensorDto(d.sensor.device_id, d.sensor.door_id, (short)d.sensor.module_id, d.sensor.input_no, d.sensor.input_mode, d.sensor.debounce, d.sensor.holdtime, d.sensor.dc_held, d.sensor.location_id, d.sensor.is_active),
            rexs = d.request_exits == null ? null : d.request_exits.Select(
                rx => new
                {
                    rx.device_id,
                    rx.door_id,
                    rx.module_id,
                    rx.input_no,
                    rx.input_mode,
                    rx.debounce,
                    rx.holdtime,
                    rx.mask_timezone,
                    rx.location_id,
                    rx.is_active

                }
            ),
            d.card_format,
            d.antipassback_mode,
            d.antipassback_in,
            d.area_in_id,
            d.antipassback_out,
            d.area_out_id,
            d.spare_tag,
            d.access_control_flag,
            d.mode,
            d.mode_detail,
            d.offline_mode,
            d.offline_mode_detail,
            d.default_mode,
            d.default_mode_detail,
            d.default_led_mode,
            d.pre_alarm,
            d.antipassback_delay,
            d.strike_t2,
            d.dc_held2,
            d.strike_follow_pulse,
            d.strike_follow_delay,
            d.n_ext_feature_type,
            d.i_lpb_sio,
            d.i_lpb_number,
            d.i_lpb_long_press,
            d.i_lpb_out_sio,
            d.i_lpb_out_num,
            d.df_filter_time,
            d.is_held_mask,
            d.is_force_mask,
            d.location_id,
            d.is_active
        }
        )
            .ToArrayAsync();

        var res = data.Select(d => new DoorDto
   (
       d.id,
       d.device_id,
       d.driver_id,
       d.name,
       d.access_config,
       d.pair_door_no,
       (int)d.direction,
       d.readers.Select(r => new ReaderDto(r.device_id,r.module_id, r.door_id,r.reader_no, r.data_format, r.keypad_mode, r.led_drive_mode, r.osdp_flag, r.osdp_baudrate, r.osdp_discover, r.osdp_tracing, r.osdp_address, r.osdp_secure_channel, r.location_id, r.is_active)).ToList(),
       d.reader_out_config,
       d.strike,
       d.sensor,
       d.rexs is null ? new List<RequestExitDto>() : d.rexs.Select(rx => new RequestExitDto(rx.device_id,rx.module_id,rx.door_id, rx.input_no, rx.input_mode, rx.debounce, rx.holdtime, rx.mask_timezone, rx.location_id, rx.is_active)).ToList(),
       d.card_format, d.antipassback_mode, d.antipassback_in, d.area_in_id, d.antipassback_out, d.area_out_id, d.spare_tag, d.access_control_flag, d.mode, d.mode_detail, d.offline_mode, d.offline_mode_detail,
   d.default_mode, d.default_mode_detail, d.default_led_mode, d.pre_alarm, d.antipassback_delay, d.strike_t2, d.dc_held2, d.strike_follow_pulse, d.strike_follow_delay,
   d.n_ext_feature_type, d.i_lpb_sio, d.i_lpb_number, d.i_lpb_long_press, d.i_lpb_out_sio, d.i_lpb_out_num,
   d.df_filter_time, d.is_held_mask, d.is_force_mask, d.location_id, d.is_active
   )).ToList();


        return res;
    }

    public async Task<IEnumerable<ModeDto>> GetDoorModeAsync()
    {
        var dtos = await context.door_mode.Select(x => new ModeDto(x.name, x.value, x.description)).ToArrayAsync();

        return dtos;
    }

    public async Task<short> GetLowestUnassignedNumberByDeviceIdAsync(int max, int device)
    {
        if (max <= 0) return -1;

        var query = context.door
            .AsNoTracking()
            .Where(x => x.device_id == device)
            .Select(x => x.driver_id);

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

    // public async Task<short> GetLowestUnassignedNumberByMacAsync(string mac, int max)
    // {
    //     if (max <= 0) return -1;

    //     var query = context.door
    //         .AsNoTracking()
    //         .Where(x => x.device_id == mac)
    //         .Select(x => x.component_id);

    //     // Handle empty table case quickly
    //     var hasAny = await query.AnyAsync();
    //     if (!hasAny)
    //         return 0; // start at 0 if table is empty

    //     // Load all numbers into memory (only the column, so it's lightweight)
    //     var numbers = await query.Distinct().OrderBy(x => x).ToListAsync();

    //     short expected = 1;
    //     foreach (var num in numbers)
    //     {
    //         if (num != expected)
    //             return expected; // found the lowest missing number
    //         expected++;
    //     }

    //     // If none missing in sequence, return next number
    //     if (expected > max) return -1;
    //     return expected;
    // }

    // public async Task<short> GetLowestUnassignedReaderNumberNoLimitAsync()
    // {
    //     var query = context.reader
    //        .AsNoTracking()
    //        .Select(x => x.component_id);

    //     // Handle empty table case quickly
    //     var hasAny = await query.AnyAsync();
    //     if (!hasAny)
    //         return 1; // start at 1 if table is empty

    //     // Load all numbers into memory (only the column, so it's lightweight)
    //     var numbers = await query.Distinct().OrderBy(x => x).ToListAsync();

    //     short expected = 1;
    //     foreach (var num in numbers)
    //     {
    //         if (num != expected)
    //             return expected; // found the lowest missing number
    //         expected++;
    //     }

    //     // If none missing in sequence, return next number
    //     return expected;
    // }

    // public async Task<short> GetLowestUnassignedSensorNumberNoLimitAsync()
    // {
    //     var query = context.sensor
    //        .AsNoTracking()
    //        .Select(x => x.component_id);

    //     // Handle empty table case quickly
    //     var hasAny = await query.AnyAsync();
    //     if (!hasAny)
    //         return 1; // start at 1 if table is empty

    //     // Load all numbers into memory (only the column, so it's lightweight)
    //     var numbers = await query.Distinct().OrderBy(x => x).ToListAsync();

    //     short expected = 1;
    //     foreach (var num in numbers)
    //     {
    //         if (num != expected)
    //             return expected; // found the lowest missing number
    //         expected++;
    //     }

    //     // If none missing in sequence, return next number
    //     return expected;
    // }

    // public async Task<short> GetLowestUnassignedStrikeNumberNoLimitAsync()
    // {
    //     var query = context.strike
    //        .AsNoTracking()
    //        .Select(x => x.component_id);

    //     // Handle empty table case quickly
    //     var hasAny = await query.AnyAsync();
    //     if (!hasAny)
    //         return 1; // start at 1 if table is empty

    //     // Load all numbers into memory (only the column, so it's lightweight)
    //     var numbers = await query.Distinct().OrderBy(x => x).ToListAsync();

    //     short expected = 1;
    //     foreach (var num in numbers)
    //     {
    //         if (num != expected)
    //             return expected; // found the lowest missing number
    //         expected++;
    //     }

    //     // If none missing in sequence, return next number
    //     return expected;
    // }

    // public async Task<short> GetLowestUnassignedRexNumberAsync()
    // {
    //     var query = context.request_exit
    //         .AsNoTracking()
    //         .Select(x => x.component_id);

    //     // Handle empty table case quickly
    //     var hasAny = await query.AnyAsync();
    //     if (!hasAny)
    //         return 1; // start at 1 if table is empty

    //     // Load all numbers into memory (only the column, so it's lightweight)
    //     var numbers = await query.Distinct().OrderBy(x => x).ToListAsync();

    //     short expected = 1;
    //     foreach (var num in numbers)
    //     {
    //         if (num != expected)
    //             return expected; // found the lowest missing number
    //         expected++;
    //     }

    //     // If none missing in sequence, return next number
    //     return expected;
    // }

    public async Task<IEnumerable<ModeDto>> GetReaderModeAsync()
    {
        var dtos = await context.reader_configuration_mode.Select(x => new ModeDto(x.name, x.value, x.description)).ToArrayAsync();

        return dtos;
    }

    public async Task<IEnumerable<ModeDto>> GetReaderOutModeAsync()
    {
        var res = await context.reader_out_configuration
           .Select(x => new ModeDto(x.name, x.value, x.description)).ToArrayAsync();

        return res;
    }

    public async Task<IEnumerable<ModeDto>> GetStrikeModeAsync()
    {
        var dtos = await context.strike_mode.Select(x => new ModeDto(x.name, x.value, x.description)).ToArrayAsync();

        return dtos;
    }

    public async Task<bool> IsAnyById(int id)
    {
        return await context.door.AnyAsync(x => x.id == id);
    }

    public async Task<IEnumerable<ModeDto>> GetDoorAccessControlFlagAsync()
    {
        var dtos = await context.door_access_control_flag
            .Select(x => new ModeDto(x.name, (short)x.value, x.description))
            .ToArrayAsync();

        return dtos;
    }

    public async Task<IEnumerable<ModeDto>> GetDoorSpareFlagAsync()
    {
        var dtos = await context.door_spare_flag
           .Select(x => new ModeDto(x.name, (short)x.value, x.description))
           .ToArrayAsync();

        return dtos;
    }

    public async Task<IEnumerable<ModeDto>> GetOsdpBaudrateAsync()
    {
        var dtos = await context.osdp_baudrate.Select(x => new ModeDto(x.name, (short)x.value, x.description)).ToArrayAsync();

        return dtos;
    }

    public async Task<IEnumerable<ModeDto>> GetOsdpAddressAsync()
    {
        var dtos = await context.osdp_address.Select(x => new ModeDto(x.name, (short)x.value, x.description)).ToArrayAsync();

        return dtos;
    }

    public async Task<Pagination<DoorDto>> GetPaginationAsync(PaginationParamsWithFilter param, int location)
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
                        EF.Functions.ILike(x.mode_detail, pattern)

                    );
                }
                else // SQL Server
                {
                    query = query.Where(x =>
                        x.name.Contains(search) ||
                        x.mode_detail.Contains(search)
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
              .Select(d => new
        {
            d.id,
            d.device_id,
            d.driver_id,
            d.name,
            d.access_config,
            d.pair_door_no,
            d.direction,
            readers = d.readers.Select(
            r => new
            {
                r.device_id,
                r.door_id,
                r.module_id,
                r.reader_no,
                r.data_format,
                r.keypad_mode,
                r.led_drive_mode,
                r.osdp_flag,
                r.osdp_baudrate,
                r.osdp_discover,
                r.osdp_tracing,
                r.osdp_address,
                r.osdp_secure_channel,
                r.location_id,
                r.is_active
            }
            ),
            d.reader_out_config,
            strike = d.strike == null ? null : new StrikeDto(d.strike.device_id, d.strike.door_id, (short)d.strike.module_id, d.strike.output_no, d.strike.relay_mode, d.offline_mode, d.strike.strike_max, d.strike.strike_min, d.strike.strike_mode, d.strike.location_id, d.strike.is_active),
            sensor = d.sensor == null ? null : new SensorDto(d.sensor.device_id, d.sensor.door_id, (short)d.sensor.module_id, d.sensor.input_no, d.sensor.input_mode, d.sensor.debounce, d.sensor.holdtime, d.sensor.dc_held, d.sensor.location_id, d.sensor.is_active),
            rexs = d.request_exits == null ? null : d.request_exits.Select(
                rx => new
                {
                    rx.device_id,
                    rx.door_id,
                    rx.module_id,
                    rx.input_no,
                    rx.input_mode,
                    rx.debounce,
                    rx.holdtime,
                    rx.mask_timezone,
                    rx.location_id,
                    rx.is_active

                }
            ),
            d.card_format,
            d.antipassback_mode,
            d.antipassback_in,
            d.area_in_id,
            d.antipassback_out,
            d.area_out_id,
            d.spare_tag,
            d.access_control_flag,
            d.mode,
            d.mode_detail,
            d.offline_mode,
            d.offline_mode_detail,
            d.default_mode,
            d.default_mode_detail,
            d.default_led_mode,
            d.pre_alarm,
            d.antipassback_delay,
            d.strike_t2,
            d.dc_held2,
            d.strike_follow_pulse,
            d.strike_follow_delay,
            d.n_ext_feature_type,
            d.i_lpb_sio,
            d.i_lpb_number,
            d.i_lpb_long_press,
            d.i_lpb_out_sio,
            d.i_lpb_out_num,
            d.df_filter_time,
            d.is_held_mask,
            d.is_force_mask,

            d.location_id,
            d.is_active
        }
        )
            .ToArrayAsync();

        var res = data.Select(d => new DoorDto
(
  d.id,
  d.device_id,
  d.driver_id,
  d.name,
  d.access_config,
  d.pair_door_no,
  (int)d.direction,
  d.readers.Select(r => new ReaderDto(r.device_id,r.module_id,r.door_id, r.reader_no, r.data_format, r.keypad_mode, r.led_drive_mode, r.osdp_flag, r.osdp_baudrate, r.osdp_discover, r.osdp_tracing, r.osdp_address, r.osdp_secure_channel, r.location_id, r.is_active)).ToList(),
  d.reader_out_config,
  d.strike,
  d.sensor,
  d.rexs is null ? new List<RequestExitDto>() : d.rexs.Select(rx => new RequestExitDto(rx.device_id,rx.module_id,rx.door_id, rx.input_no, rx.input_mode, rx.debounce, rx.holdtime, rx.mask_timezone, rx.location_id, rx.is_active)).ToList(),
  d.card_format, d.antipassback_mode, d.antipassback_in, d.area_in_id, d.antipassback_out, d.area_out_id, d.spare_tag, d.access_control_flag, d.mode, d.mode_detail, d.offline_mode, d.offline_mode_detail,
d.default_mode, d.default_mode_detail, d.default_led_mode, d.pre_alarm, d.antipassback_delay, d.strike_t2, d.dc_held2, d.strike_follow_pulse, d.strike_follow_delay,
d.n_ext_feature_type, d.i_lpb_sio, d.i_lpb_number, d.i_lpb_long_press, d.i_lpb_out_sio, d.i_lpb_out_num,
d.df_filter_time, d.is_held_mask, d.is_force_mask,d.location_id, d.is_active
)).ToList();


        return new Pagination<DoorDto>
        {
            Data = res,
            Page = new PaginationData
            {
                TotalCount = count,
                PageNumber = param.PageNumber,
                PageSize = param.PageSize,
                TotalPage = (int)Math.Ceiling(count / (double)param.PageSize)
            }
        };
    }

    public async Task<bool> IsAnyByNameAsync(string name)
    {
        return await context.door.AnyAsync(x => x.name.Equals(name));
    }


}
