using Aero.Application.DTOs;
using Aero.Domain.Entities;
using Aero.Domain.Interface;
using Aero.Infrastructure.Persistences;
using Aero.Infrastructure.Mapper;
using Microsoft.EntityFrameworkCore;
using System;
using Aero.Application.Interface;

namespace Aero.Infrastructure.Repositories;

public class CpRepository(AppDbContext context) : ICpRepository
{
      public async Task<int> AddAsync(ControlPoint data)
      {
            var en = ControlPointMapper.ToEf(data);
            await context.control_point.AddAsync(en);
            return await context.SaveChangesAsync();
      }

      public async Task<int> DeleteByComponentIdAsync(short component)
      {
            var en = await context.control_point
            .Where(x => x.component_id == component)
            .FirstOrDefaultAsync();

            if(en is null) return 0;

            context.control_point.Remove(en);
            return await context.SaveChangesAsync();
      }

      public async Task<int> UpdateAsync(ControlPoint newData)
      {
            var en = await context.control_point
            .Where(x => x.component_id == newData.ComponentId)
            .FirstOrDefaultAsync();

            if(en is null) return 0;

            ControlPointMapper.Update(en,newData);

            context.control_point.Update(en);
            return await context.SaveChangesAsync();
      }

    public async Task<int> CountByMacAndUpdateTimeAsync(string mac, DateTime sync)
    {
        var res = await context.control_point
        .AsNoTracking()
        .Where(x => x.module.mac.Equals(mac) && x.updated_date > sync)
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
            ComponentId = x.component_id,
            HardwareName = x.module.hardware.name,
            Mac = x.module.hardware_mac,
            LocationId = x.location_id,
            IsActive = x.is_active,

            // extend_desc
            Name = x.name,
            CpId = x.cp_id,
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
            .Where(sio => sio.mac == mac && sio.component_id == ModuleId)
            .Select(cp => cp.n_output)
            .FirstOrDefaultAsync();

        var strk = await context.strike
            .AsNoTracking()
            .Where(x => x.module_id == ModuleId && x.module.mac == mac)
            .Select(x => x.output_no)
            .ToArrayAsync();

        var cp = await context.control_point
            .AsNoTracking()
            .Where(x => x.module_id == ModuleId && x.module.mac == mac)
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
            ComponentId = x.component_id,
            HardwareName = x.module.hardware.name,
            Mac = x.module.hardware_mac,
            LocationId = x.location_id,
            IsActive = x.is_active,

            // extend_desc
            Name = x.name,
            CpId = x.cp_id,
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
        .Where(x => x.location_id == locationId || x.location_id == 1)
        .OrderBy(x => x.component_id)
        .Select(x => new ControlPointDto
        {
            // Base
            ComponentId = x.component_id,
            HardwareName = x.module.hardware.name,
            Mac = x.module.hardware_mac,
            LocationId = x.location_id,
            IsActive = x.is_active,

            // extend_desc
            Name = x.name,
            CpId = x.cp_id,
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
           .Where(x => x.module.mac == mac && x.component_id == component)
          .Select(x => new ControlPointDto
          {
              // Base
              ComponentId = x.component_id,
              HardwareName = x.module.hardware.name,
              Mac = x.module.hardware_mac,
              LocationId = x.location_id,
              IsActive = x.is_active,

              // extend_desc
              Name = x.name,
              ModuleId = x.module_id,
              CpId = x.cp_id,
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
            ComponentId = x.component_id,
            HardwareName = x.module.hardware.name,
            Mac = x.module.hardware_mac,
            LocationId = x.location_id,
            IsActive = x.is_active,

            // extend_desc
            Name = x.name,
            CpId = x.cp_id,
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

    public async Task<short> GetLowestUnassignedNumberAsync(int max, string mac)
    {
        if (string.IsNullOrEmpty(mac))
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
        else
        {
            if (max <= 0) return -1;

            var query = context.control_point
                .AsNoTracking()
                .Where(x => x.mac == mac)
                .Select(x => x.cp_id);

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

    public async Task<Pagination<ControlPointDto>> GetPaginationAsync(PaginationParamsWithFilter param, short location)
    {

        var query = context.control_point.AsNoTracking().AsQueryable();


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
                        EF.Functions.ILike(x.module_desc, pattern) ||
                        EF.Functions.ILike(x.relay_mode_desc, pattern) ||
                        EF.Functions.ILike(x.offline_mode_desc, pattern)

                    );
                }
                else // SQL Server
                {
                    query = query.Where(x =>
                        x.name.Contains(search) ||
                        x.module_desc.Contains(search) ||
                        x.relay_mode_desc.Contains(search) ||
                        x.offline_mode_desc.Contains(search)
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
             .Select(x => new ControlPointDto
             {
                 // Base
                 ComponentId = x.component_id,
                 HardwareName = x.module.hardware.name,
                 Mac = x.module.mac,
                 LocationId = x.location_id,
                 IsActive = x.is_active,

                 // extend_desc
                 Name = x.name,
                 ModuleId = x.module_id,
                 CpId = x.cp_id,
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


        return new Pagination<ControlPointDto>
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

    public async Task<bool> IsAnyByComponentId(short component)
    {
        return await context.control_point.AsNoTracking().AnyAsync(x => x.component_id == component);
    }
}
