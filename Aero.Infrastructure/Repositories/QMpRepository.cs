using System;
using Aero.Application.DTOs;
using Aero.Application.Interfaces;
using Aero.Domain.Entities;
using Aero.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Aero.Infrastructure.Repositories;

public class QMpRepository(AppDbContext context) : IQMpRepository
{
      public async Task<int> CountByMacAndUpdateTimeAsync(string mac, DateTime sync)
      {
            var res = await context.monitor_point.AsNoTracking()
            .Where(x => x.module.hardware_mac.Equals(mac) && x.updated_date > sync)
            .CountAsync();

            return res;
      }

      public async Task<IEnumerable<MonitorPointDto>> GetAsync()
      {
            var res = await context.monitor_point
            .AsNoTracking()
            .OrderBy(x => x.component_id)
            .Select(x => new MonitorPointDto
            {
                  // Base 
                  ComponentId = x.component_id,
                  Mac = x.module.hardware.mac,
                  HardwareName = x.module.hardware.name,
                  LocationId = x.location_id,
                  IsActive = x.is_active,

                  // extend_desc 
                  Name = x.name,
                  ModuleId = x.module_id,
                  ModuleDescription = x.module.model_desc,
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

            })
            .ToArrayAsync();

            return res;
      }

      public async Task<IEnumerable<short>> GetAvailableIpAsync(string mac, short moduleId)
      {
            var input = await context.module
                .AsNoTracking()
                .Where(mp => mp.component_id == moduleId && mp.hardware_mac == mac)
                .Select(mp => mp.n_input)
                .FirstOrDefaultAsync();

            var sensors = await context.sensor
                .AsNoTracking()
                .Where(x => x.module_id == moduleId && x.module.hardware_mac == mac)
                .Select(x => x.input_no)
                .ToArrayAsync();

            var rex = await context.request_exit
                .AsNoTracking()
                .Where(x => x.module_id == moduleId && x.module.hardware_mac == mac)
                .Select(x => x.input_no)
                .ToArrayAsync();

            var mp = await context.monitor_point
                .AsNoTracking()
                .Where(x => x.module_id == moduleId && x.module.hardware_mac == mac)
                .Select(x => x.input_no)
                .ToArrayAsync();


            var unavailable = sensors
                .Union(rex)
                .Union(mp)
                .Distinct()
                .ToList();


            List<short> all = Enumerable.Range(0, input - 3).Select(i => (short)i).ToList();
            IEnumerable<short> av = all.Except(unavailable).ToArray();
            return av;
      }

      public async Task<MonitorPointDto> GetByComponentIdAsync(short componentId)
      {
            var res = await context.monitor_point
            .AsNoTracking()
            .Where(x => x.component_id == componentId)
            .OrderBy(x => x.component_id)
            .Select(x => new MonitorPointDto
            {
                  // Base 
                  ComponentId = x.component_id,
                  Mac = x.module.hardware.mac,
                  HardwareName = x.module.hardware.name,
                  LocationId = x.location_id,
                  IsActive = x.is_active,

                  // extend_desc 
                  Name = x.name,
                  ModuleId = x.module_id,
                  ModuleDescription = x.module.model_desc,
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

            })
            .FirstOrDefaultAsync();

            return res;
      }

      public async Task<IEnumerable<MonitorPointDto>> GetByLocationIdAsync(short locationId)
      {
            var res = await context.monitor_point
            .AsNoTracking()
            .Where(x => x.location_id == locationId)
            .OrderBy(x => x.component_id)
            .Select(x => new MonitorPointDto
            {
                  // Base 
                  ComponentId = x.component_id,
                  Mac = x.module.hardware.mac,
                  HardwareName = x.module.hardware.name,
                  LocationId = x.location_id,
                  IsActive = x.is_active,

                  // extend_desc 
                  Name = x.name,
                  ModuleId = x.module_id,
                  ModuleDescription = x.module.model_desc,
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

            })
            .ToArrayAsync();

            return res;
      }

      public async Task<IEnumerable<MonitorPointDto>> GetByMacAsync(string mac)
      {
            var res = await context.monitor_point
            .AsNoTracking()
            .Where(x => x.module.hardware_mac.Equals(mac))
            .OrderBy(x => x.component_id)
            .Select(x => new MonitorPointDto
            {
                  // Base 
                  ComponentId = x.component_id,
                  Mac = x.module.hardware.mac,
                  HardwareName = x.module.hardware.name,
                  LocationId = x.location_id,
                  IsActive = x.is_active,

                  // extend_desc 
                  Name = x.name,
                  ModuleId = x.module_id,
                  ModuleDescription = x.module.model_desc,
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

            })
            .ToArrayAsync();

            return res;
      }


      public async Task<IEnumerable<Mode>> GetInputModeAsync()
      {
            var res = await context.input_mode.AsNoTracking().Select(x => new Mode
            {
                  Name = x.name,
                  Value = x.value,
                  Description = x.description,
            }).ToArrayAsync();

            return res;
      }

      public async Task<IEnumerable<Mode>> GetLogFunctionAsync()
      {
            var dtos = await context.monitor_point_log_function
                .AsNoTracking()
                .Select(x => new Mode 
                {
                    Name = x.name,
                    Value = x.value,
                    Description = x.description,
                })
                .ToArrayAsync();

                return dtos;
      }

      public async Task<short> GetLowestUnassignedNumberAsync(int max, string mac)
      {
            if (string.IsNullOrEmpty(mac))
            {
                  if (max <= 0) return -1;

                  var query = context.monitor_point
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
            else
            {
                  if (max <= 0) return -1;

                  var query = context.monitor_point
                      .AsNoTracking()
                      .Where(x => x.mac == mac)
                      .Select(x => x.mp_id);

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
      }

      public async Task<string> GetMacFromComponentIdAsync(short component)
      {
            return await context.monitor_point
            .AsNoTracking()
            .Where(x => x.component_id == component)
            .OrderBy(x => x.component_id)
            .Select(x => x.mac)
            .FirstOrDefaultAsync() ?? "";
      }

      public async Task<IEnumerable<Mode>> GetMonitorPointModeAsync()
      {
            var res = await context.monitor_point_mode.AsNoTracking().Select(x => new Mode
            {
                  Name = x.name,
                  Value = x.value,
                  Description = x.description,
            }).ToArrayAsync();

            return res;
      }

      public async Task<bool> IsAnyByComponentId(short component)
      {
            return await context.monitor_point.AnyAsync(x => x.component_id == component);
      }

      public async Task<bool> IsAnyByMacAndComponentIdAsync(string mac, short component)
      {
            return await context.monitor_point.AnyAsync(x => x.mac.Equals(mac) && x.component_id == component);
      }
}
