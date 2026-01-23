using System;
using Aero.Application.DTOs;
using Aero.Application.Interfaces;
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
                  Uuid = x.uuid,
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

      public async Task<MonitorPointDto> GetByComponentIdAsync(short componentId)
      {
            var res = await context.monitor_point
            .AsNoTracking()
            .Where(x => x.component_id == componentId)
            .OrderBy(x => x.component_id)
            .Select(x => new MonitorPointDto
            {
                  // Base 
                  Uuid = x.uuid,
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
                  Uuid = x.uuid,
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
                  Uuid = x.uuid,
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

      public Task<short> GetLowestUnassignedNumberAsync(int max)
      {
            throw new NotImplementedException();
      }

      public Task<bool> IsAnyByComponentId(short component)
      {
            throw new NotImplementedException();
      }
}
