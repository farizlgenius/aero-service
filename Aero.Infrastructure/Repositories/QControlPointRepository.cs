using System;
using Aero.Application.DTOs;
using Aero.Application.Interfaces;
using Aero.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Aero.Infrastructure.Repositories;

public class QControlPointRepository(AppDbContext context) : IQCpRepository
{
      public async Task<int> CountByMacAndUpdateTimeAsync(string mac, DateTime sync)
      {
            var res = await context.control_point
            .AsNoTracking()
            .Where(x => x.module.hardware_mac.Equals(mac) && x.updated_date > sync)
            .CountAsync();

            return res;
      }

      public async Task<IEnumerable<ControlPointDto>> GetAsync()
      {
            var res = await context.control_point
            .AsNoTracking()
            .OrderBy(x => x.component_id)
            .Select(x => new ControlPointDto
            {
                  // Base
                  Uuid = x.uuid,
                  ComponentId = x.component_id,
                  HardwareName = x.module.hardware.name,
                  Mac = x.module.hardware_mac,
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
            })
            .ToArrayAsync();

            return res;
      }

      public async Task<IEnumerable<short>> GetAvailableOpAsync(string mac, short ModuleId)
      {
            var ops = await context.module
                .AsNoTracking()
                .Where(sio => sio.hardware_mac == mac && sio.component_id == ModuleId)
                .Select(cp => cp.n_output)
                .FirstOrDefaultAsync();

            var strk = await context.strike
                .AsNoTracking()
                .Where(x => x.module_id == ModuleId && x.module.hardware_mac == mac)
                .Select(x => x.output_no)
                .ToArrayAsync();

            var cp = await context.control_point
                .AsNoTracking()
                .Where(x => x.module_id == ModuleId && x.module.hardware_mac == mac)
                .Select(x => x.output_no)
                .ToArrayAsync();


            var unavailable = strk
                .Union(cp)
                .Distinct()
                .ToList();
            
            List<short> all = Enumerable.Range(0, ops).Select(x => (short)x).ToList();
            return all.Except(unavailable).ToList();
      }

      public async Task<ControlPointDto> GetByComponentIdAsync(short componentId)
      {
            var res = await context.control_point
            .AsNoTracking()
            .Where(x => x.component_id == componentId)
            .OrderBy(x => x.component_id)
            .Select(x => new ControlPointDto
            {
                  // Base
                  Uuid = x.uuid,
                  ComponentId = x.component_id,
                  HardwareName = x.module.hardware.name,
                  Mac = x.module.hardware_mac,
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
            })
            .FirstOrDefaultAsync();

            return res;
      }

      public async Task<IEnumerable<ControlPointDto>> GetByLocationIdAsync(short locationId)
      {
            var res = await context.control_point
            .AsNoTracking()
            .Where(x => x.location_id == locationId)
            .OrderBy(x => x.component_id)
            .Select(x => new ControlPointDto
            {
                  // Base
                  Uuid = x.uuid,
                  ComponentId = x.component_id,
                  HardwareName = x.module.hardware.name,
                  Mac = x.module.hardware_mac,
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
            })
            .ToArrayAsync();

            return res;
      }

      public async Task<ControlPointDto> GetByMacAndComponentIdAsync(string mac, short component)
      {
             var dto = await context.control_point
                .Where(x => x.module.hardware_mac == mac && x.component_id == component)
               .Select(x => new ControlPointDto
               {
                   // Base
                   Uuid = x.uuid,
                   ComponentId = x.component_id,
                   HardwareName = x.module.hardware.name,
                   Mac = x.module.hardware_mac,
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
               })
               .FirstOrDefaultAsync();

               return dto;


      }

      public async Task<IEnumerable<ControlPointDto>> GetByMacAsync(string mac)
      {
            var res = await context.control_point
            .AsNoTracking()
            .OrderBy(x => x.component_id)
            .Where(x => x.module.hardware_mac.Equals(mac))
            .Select(x => new ControlPointDto
            {
                  // Base
                  Uuid = x.uuid,
                  ComponentId = x.component_id,
                  HardwareName = x.module.hardware.name,
                  Mac = x.module.hardware_mac,
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
            })
            .ToArrayAsync();

            return res;
      }

      public async Task<short> GetLowestUnassignedNumberAsync(int max)
      {
            if (max <= 0) return -1;

            var query = context.control_point
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

      public async Task<short> GetModeNoByOfflineAndRelayModeAsync(short offlineMode, short relayMode)
      {
            var res = await context.output_mode
            .AsNoTracking()
            .OrderBy(x => x.id)
            .Where(x => x.offline_mode == offlineMode && x.relay_mode == relayMode)
            .Select(x => x.value)
            .FirstOrDefaultAsync();

            return res;
      }

      public async Task<IEnumerable<Mode>> GetOfflineModeAsync()
      {
            var dtos = await context.relay_offline_mode.AsNoTracking().Select(x => new Mode 
            {
                Name = x.name,
                Value = x.value,
                Description = x.description,
            }).ToArrayAsync();

            return dtos;
      }

      public async Task<IEnumerable<Mode>> GetRelayModeAsync()
      {
             var dtos = await context.relay_mode.AsNoTracking().Select(x => new Mode 
            {
                Name = x.name,
                Value = x.value,
                Description = x.description,
            }).ToArrayAsync();
            return dtos;
      }

      public async Task<bool> IsAnyByComponentId(short component)
      {
            return await context.control_point.AsNoTracking().AnyAsync(x => x.component_id == component);
      }
}
