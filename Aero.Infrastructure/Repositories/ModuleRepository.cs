using System;
using System.IO.Compression;
using Aero.Application.DTOs;
using Aero.Application.Interfaces;
using Aero.Domain.Entities;
using Aero.Domain.Enums;
using Aero.Domain.Interfaces;
using Aero.Infrastructure.Persistences;
using HID.Aero.ScpdNet.Wrapper;
using Microsoft.EntityFrameworkCore;

namespace Aero.Infrastructure.Repositories;

public sealed class ModuleRepository(AppDbContext context) : IModuleRepository
{
      public Task<int> AddAsync(Module data)
      {
            throw new NotImplementedException();
      }


      public Task<int> UpdateAsync(Module newData)
      {
            throw new NotImplementedException();
      }

    // public async Task HandleFoundModuleAsync(SCPReplyMessage message)
    //   {
    //       if(message.tran.s_comm.comm_sts == 3)
    //       {
    //           if (await context.module.AsNoTracking().AnyAsync(x => x.serial_number.Equals(message.tran.s_comm.ser_num.ToString()))) return;
    //           var id = await helperService.GetLowestUnassignedNumberAsync<Module>(context, 16);
    //           if (id == 0) return;
    //           var mac = await helperService.GetMacFromIdAsync((short)message.SCPId);
    //           var module = new Module
    //           {
    //               // Base 
    //               component_id = id,
    //               hardware_mac = mac,
    //               location_id = await context.hardware.AsNoTracking().Where(x => x.mac.Equals(mac)).OrderBy(x => x.id).Select(x => x.location_id).FirstOrDefaultAsync(),
    //               is_active = true,
    //               created_date = DateTime.UtcNow,
    //               updated_date = DateTime.UtcNow,

    //               // extend_desc
    //               model = message.tran.s_comm.model,
    //               model_desc = Domain.Enums.Model.AeroX1100.ToString(),
    //               revision = ((int)message.tran.s_comm.revision).ToString(),
    //               address = -1,
    //               port = 3,
    //               n_input = (short)InputComponents.HIDAeroX1100,
    //               n_output = (short)OutputComponents.HIDAeroX1100,
    //               n_reader = (short)ReaderComponents.HIDAeroX1100,
    //               msp1_no = 0,
    //               baudrate = -1,
    //               n_protocol = 0,
    //               n_dialect = 0,

    //           };
    //       }else if(message.tran.s_comm.comm_sts == 2)
    //       {

    //       }

    //   }

    public async Task<int> CountByDeviceIdAndUpdateTimeAsync(int device, DateTime sync)
    {
        var res = await context.module
        .AsNoTracking()
        .Where(x => x.device_id == device && x.updated_date > sync)
        .CountAsync();

        return res;
    }

    public async Task<IEnumerable<ModuleDto>> GetAsync()
    {
        var res = await context.module
            .AsNoTracking()
            .Select(m => new ModuleDto(
                m.driver_id,
                m.model,
                m.model_detail,
                m.revision,
                m.serial_number,
                m.n_hardware_id,
                m.n_hardware_id_detail,
                m.n_hardware_rev,
                m.n_product_id,
                m.n_product_ver,
                m.n_enc_config,
                m.n_enc_config_detail,
                m.n_enc_key_status,
                m.n_enc_key_status_detail,
                m.readers == null ? null : m.readers.Select(r => new ReaderDto(
                    r.device_id,
                    r.module_id,
                    r.door_id,
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
                )).ToList(),
                m.sensors == null ? null : m.sensors.Select(s => new SensorDto(
                    s.device_id,
                    s.module_id,
                    s.door_id,
                    s.input_no,
                    s.input_mode,
                    s.debounce,
                    s.holdtime,
                    s.dc_held,
                    s.location_id,
                    s.is_active
                )).ToList(),
                m.strikes == null ? null : m.strikes.Select(k => new StrikeDto(
                    k.device_id,
                    k.door_id,
                    k.module_id,
                    k.output_no,
                    k.relay_mode,
                    k.offline_mode,
                    k.strike_max,
                    k.strike_min,
                    k.strike_mode,
                    k.location_id,
                    k.is_active
                )).ToList(),
                m.request_exits == null ? null : m.request_exits.Select(x => new RequestExitDto(
                    x.device_id,
                    x.module_id,
                    x.door_id,
                    x.input_no,
                    x.input_mode,
                    x.debounce,
                    x.holdtime,
                    x.mask_timezone,
                    x.location_id,
                    x.is_active
                )).ToList(),
                m.monitor_points == null ? null : m.monitor_points.Select(m => new MonitorPointDto(
                    m.id,
                    m.device_id,
                    m.driver_id,
                    m.name,
                    (short)m.module_id,
                    m.module.model_detail,
                    m.input_no,
                    m.input_mode,
                    m.input_mode_detail,
                    m.debounce,
                    m.holdtime,
                    m.log_function,
                    m.log_function_detail,
                    m.monitor_point_mode,
                    m.monitor_point_mode_detail,
                    m.delay_entry,
                    m.delay_exit,
                    m.is_mask,
                    m.location_id,
                    m.is_active
                )).ToList(),
                m.control_points == null ? null : m.control_points.Select(c => new ControlPointDto(
                    c.id,
                    c.driver_id,
                    c.name,
                    c.module_id,
                    c.module_detail,
                    c.output_no,
                    c.relaymode,
                    c.relaymode_detail,
                    c.offlinemode,
                    c.offlinemode_detail,
                    c.default_pulse,
                    c.device_id,
                    c.location_id,
                    c.is_active
                )).ToList(),
                m.address,
                m.address_detail,
                m.port,
                m.n_input,
                m.n_output,
                m.n_reader,
                m.msp1_no,
                m.baudrate,
                m.n_protocol,
                m.n_dialect,
                m.location_id,
                m.is_active
            ))
            .ToArrayAsync();

        return res;
    }

    public async Task<IEnumerable<ModeDto>> GetBaudrateAsync()
    {
        var res = await context.module_baudrate
            .AsNoTracking()
            .Select(x => new ModeDto(x.name,(short)x.value,x.description))
            .ToArrayAsync();

        return res;
    }

    public Task<ModuleDto> GetByComponentIdAsync(short componentId)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<ModuleDto>> GetByLocationIdAsync(short locationId)
    {
        var res = await context.module
            .AsNoTracking()
            .Where(x => x.location_id == locationId || x.location_id == 1)
           .Select(m => new ModuleDto(
                m.driver_id,
                m.model,
                m.model_detail,
                m.revision,
                m.serial_number,
                m.n_hardware_id,
                m.n_hardware_id_detail,
                m.n_hardware_rev,
                m.n_product_id,
                m.n_product_ver,
                m.n_enc_config,
                m.n_enc_config_detail,
                m.n_enc_key_status,
                m.n_enc_key_status_detail,
                m.readers == null ? null : m.readers.Select(r => new ReaderDto(
                    r.device_id,
                    r.module_id,
                    r.door_id,
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
                )).ToList(),
                m.sensors == null ? null : m.sensors.Select(s => new SensorDto(
                    s.device_id,
                    s.module_id,
                    s.door_id,
                    s.input_no,
                    s.input_mode,
                    s.debounce,
                    s.holdtime,
                    s.dc_held,
                    s.location_id,
                    s.is_active
                )).ToList(),
                m.strikes == null ? null : m.strikes.Select(k => new StrikeDto(
                    k.device_id,
                    k.door_id,
                    k.module_id,
                    k.output_no,
                    k.relay_mode,
                    k.offline_mode,
                    k.strike_max,
                    k.strike_min,
                    k.strike_mode,
                    k.location_id,
                    k.is_active
                )).ToList(),
                m.request_exits == null ? null : m.request_exits.Select(x => new RequestExitDto(
                    x.device_id,
                    x.module_id,
                    x.door_id,
                    x.input_no,
                    x.input_mode,
                    x.debounce,
                    x.holdtime,
                    x.mask_timezone,
                    x.location_id,
                    x.is_active
                )).ToList(),
                m.monitor_points == null ? null : m.monitor_points.Select(m => new MonitorPointDto(
                    m.id,
                    m.device_id,
                    m.driver_id,
                    m.name,
                    (short)m.module_id,
                    m.module.model_detail,
                    m.input_no,
                    m.input_mode,
                    m.input_mode_detail,
                    m.debounce,
                    m.holdtime,
                    m.log_function,
                    m.log_function_detail,
                    m.monitor_point_mode,
                    m.monitor_point_mode_detail,
                    m.delay_entry,
                    m.delay_exit,
                    m.is_mask,
                    m.location_id,
                    m.is_active
                )).ToList(),
                m.control_points == null ? null : m.control_points.Select(c => new ControlPointDto(
                    c.id,
                    c.driver_id,
                    c.name,
                    c.module_id,
                    c.module_detail,
                    c.output_no,
                    c.relaymode,
                    c.relaymode_detail,
                    c.offlinemode,
                    c.offlinemode_detail,
                    c.default_pulse,
                    c.device_id,
                    c.location_id,
                    c.is_active
                )).ToList(),
                m.address,
                m.address_detail,
                m.port,
                m.n_input,
                m.n_output,
                m.n_reader,
                m.msp1_no,
                m.baudrate,
                m.n_protocol,
                m.n_dialect,
                m.location_id,
                m.is_active
            )).ToArrayAsync();

        return res;
    }

    public async Task<IEnumerable<ModuleDto>> GetByDeviceIdAsync(int device)
    {
        var res = await context.module
        .AsNoTracking()
        .Where(x => x.device_id == device)
        .OrderBy(x => x.id)
        .Select(m => new ModuleDto(
                m.driver_id,
                m.model,
                m.model_detail,
                m.revision,
                m.serial_number,
                m.n_hardware_id,
                m.n_hardware_id_detail,
                m.n_hardware_rev,
                m.n_product_id,
                m.n_product_ver,
                m.n_enc_config,
                m.n_enc_config_detail,
                m.n_enc_key_status,
                m.n_enc_key_status_detail,
                m.readers == null ? null : m.readers.Select(r => new ReaderDto(
                    r.device_id,
                    r.module_id,
                    r.door_id,
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
                )).ToList(),
                m.sensors == null ? null : m.sensors.Select(s => new SensorDto(
                    s.device_id,
                    s.module_id,
                    s.door_id,
                    s.input_no,
                    s.input_mode,
                    s.debounce,
                    s.holdtime,
                    s.dc_held,
                    s.location_id,
                    s.is_active
                )).ToList(),
                m.strikes == null ? null : m.strikes.Select(k => new StrikeDto(
                    k.device_id,
                    k.door_id,
                    k.module_id,
                    k.output_no,
                    k.relay_mode,
                    k.offline_mode,
                    k.strike_max,
                    k.strike_min,
                    k.strike_mode,
                    k.location_id,
                    k.is_active
                )).ToList(),
                m.request_exits == null ? null : m.request_exits.Select(x => new RequestExitDto(
                    x.device_id,
                    x.module_id,
                    x.door_id,
                    x.input_no,
                    x.input_mode,
                    x.debounce,
                    x.holdtime,
                    x.mask_timezone,
                    x.location_id,
                    x.is_active
                )).ToList(),
                m.monitor_points == null ? null : m.monitor_points.Select(m => new MonitorPointDto(
                    m.id,
                    m.device_id,
                    m.driver_id,
                    m.name,
                    (short)m.module_id,
                    m.module.model_detail,
                    m.input_no,
                    m.input_mode,
                    m.input_mode_detail,
                    m.debounce,
                    m.holdtime,
                    m.log_function,
                    m.log_function_detail,
                    m.monitor_point_mode,
                    m.monitor_point_mode_detail,
                    m.delay_entry,
                    m.delay_exit,
                    m.is_mask,
                    m.location_id,
                    m.is_active
                )).ToList(),
                m.control_points == null ? null : m.control_points.Select(c => new ControlPointDto(
                    c.id,
                    c.driver_id,
                    c.name,
                    c.module_id,
                    c.module_detail,
                    c.output_no,
                    c.relaymode,
                    c.relaymode_detail,
                    c.offlinemode,
                    c.offlinemode_detail,
                    c.default_pulse,
                    c.device_id,
                    c.location_id,
                    c.is_active
                )).ToList(),
                m.address,
                m.address_detail,
                m.port,
                m.n_input,
                m.n_output,
                m.n_reader,
                m.msp1_no,
                m.baudrate,
                m.n_protocol,
                m.n_dialect,
                m.location_id,
                m.is_active
            ))
        .ToArrayAsync();

        return res;
    }

    public async Task<short> GetLowestUnassignedNumberAsync(int max, int device)
    {
        if (max <= 0) return -1;

            var query = context.module
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

    public async Task<IEnumerable<ModeDto>> GetProtocolAsync()
    {
        var res = await context.module_protocol
            .AsNoTracking()
            .Select(x => new ModeDto(x.name,x.value,x.description))
            .ToArrayAsync();

        return res;
    }

    public async Task<Pagination<ModuleDto>> GetPaginationAsync(PaginationParamsWithFilter param, int location)
    {

        var query = context.module.AsNoTracking().AsQueryable();


        if (!string.IsNullOrWhiteSpace(param.Search))
        {
            if (!string.IsNullOrWhiteSpace(param.Search))
            {
                var search = param.Search.Trim();

                if (context.Database.IsNpgsql())
                {
                    var pattern = $"%{search}%";

                    query = query.Where(x =>
                        EF.Functions.ILike(x.model_detail, pattern) ||
                        EF.Functions.ILike(x.serial_number, pattern) ||
                        EF.Functions.ILike(x.address_detail, pattern)

                    );
                }
                else // SQL Server
                {
                    query = query.Where(x =>
                        x.model_detail.Contains(search) ||
                        x.serial_number.Contains(search) ||
                        x.address_detail.Contains(search)
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
             .Select(m => new ModuleDto(
                m.driver_id,
                m.model,
                m.model_detail,
                m.revision,
                m.serial_number,
                m.n_hardware_id,
                m.n_hardware_id_detail,
                m.n_hardware_rev,
                m.n_product_id,
                m.n_product_ver,
                m.n_enc_config,
                m.n_enc_config_detail,
                m.n_enc_key_status,
                m.n_enc_key_status_detail,
                m.readers == null ? null : m.readers.Select(r => new ReaderDto(
                    r.device_id,
                    r.module_id,
                    r.door_id,
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
                )).ToList(),
                m.sensors == null ? null : m.sensors.Select(s => new SensorDto(
                    s.device_id,
                    s.module_id,
                    s.door_id,
                    s.input_no,
                    s.input_mode,
                    s.debounce,
                    s.holdtime,
                    s.dc_held,
                    s.location_id,
                    s.is_active
                )).ToList(),
                m.strikes == null ? null : m.strikes.Select(k => new StrikeDto(
                    k.device_id,
                    k.door_id,
                    k.module_id,
                    k.output_no,
                    k.relay_mode,
                    k.offline_mode,
                    k.strike_max,
                    k.strike_min,
                    k.strike_mode,
                    k.location_id,
                    k.is_active
                )).ToList(),
                m.request_exits == null ? null : m.request_exits.Select(x => new RequestExitDto(
                    x.device_id,
                    x.module_id,
                    x.door_id,
                    x.input_no,
                    x.input_mode,
                    x.debounce,
                    x.holdtime,
                    x.mask_timezone,
                    x.location_id,
                    x.is_active
                )).ToList(),
                m.monitor_points == null ? null : m.monitor_points.Select(m => new MonitorPointDto(
                    m.id,
                    m.device_id,
                    m.driver_id,
                    m.name,
                    (short)m.module_id,
                    m.module.model_detail,
                    m.input_no,
                    m.input_mode,
                    m.input_mode_detail,
                    m.debounce,
                    m.holdtime,
                    m.log_function,
                    m.log_function_detail,
                    m.monitor_point_mode,
                    m.monitor_point_mode_detail,
                    m.delay_entry,
                    m.delay_exit,
                    m.is_mask,
                    m.location_id,
                    m.is_active
                )).ToList(),
                m.control_points == null ? null : m.control_points.Select(c => new ControlPointDto(
                    c.id,
                    c.driver_id,
                    c.name,
                    c.module_id,
                    c.module_detail,
                    c.output_no,
                    c.relaymode,
                    c.relaymode_detail,
                    c.offlinemode,
                    c.offlinemode_detail,
                    c.default_pulse,
                    c.device_id,
                    c.location_id,
                    c.is_active
                )).ToList(),
                m.address,
                m.address_detail,
                m.port,
                m.n_input,
                m.n_output,
                m.n_reader,
                m.msp1_no,
                m.baudrate,
                m.n_protocol,
                m.n_dialect,
                m.location_id,
                m.is_active
            ))
            .ToListAsync();


        return new Pagination<ModuleDto>
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



      public async Task<bool> IsAnyByDriverAndDeviceIdAsnyc(int device, short driver)
      {
            return await context.module.AnyAsync(x => x.device_id == device && x.driver_id == driver);
      }



      public Task<IEnumerable<ModuleDto>> GetAnyByDeviceId(int device)
      {
            throw new NotImplementedException();
      }

      public Task<int> DeleteByIdAsync(int id)
      {
            throw new NotImplementedException();
      }

      public Task<bool> IsAnyByIdAsync(int id)
      {
            throw new NotImplementedException();
      }

      public Task<ModuleDto> GetByIdAsync(int id)
      {
            throw new NotImplementedException();
      }

      public Task<IEnumerable<ModuleDto>> GetByLocationIdAsync(int locationId)
      {
            throw new NotImplementedException();
      }


      public Task<bool> IsAnyByNameAsync(string name)
      {
            throw new NotImplementedException();
      }
}
