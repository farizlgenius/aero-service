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

      public Task<ControlPointDto> GetByComponentIdAsync(short componentId)
      {
            throw new NotImplementedException();
      }

      public Task<IEnumerable<ControlPointDto>> GetByLocationIdAsync(short locationId)
      {
            throw new NotImplementedException();
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

      public Task<short> GetLowestUnassignedNumberAsync(int max)
      {
            throw new NotImplementedException();
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

      public Task<bool> IsAnyByComponet(short component)
      {
            throw new NotImplementedException();
      }
}
