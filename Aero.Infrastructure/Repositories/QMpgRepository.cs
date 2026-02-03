using System;
using Aero.Application.DTOs;
using Aero.Domain.Entities;
using Aero.Domain.Interface;
using Aero.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Aero.Infrastructure.Repositories;

public class QMpgRepository(AppDbContext context) : IQMpgRepository
{
      public async Task<int> CountByMacAndUpdateTimeAsync(string mac, DateTime sync)
      {
            var res = await context.monitor_group
            .AsNoTracking()
            .Where(x => x.hardware_mac.Equals(mac) && x.updated_date > sync)
            .CountAsync();

            return res;
      }

      public async Task<IEnumerable<MonitorGroupDto>> GetAsync()
      {
            var res = await context.monitor_group
            .AsNoTracking()
            .Select(en => new MonitorGroupDto
            {
                  // Base 
                  ComponentId = en.component_id,
                  Mac = en.hardware_mac,
                  LocationId = en.location_id,
                  IsActive = en.is_active,

                  // Detail
                  Name = en.name,
                  nMpCount = en.n_mp_count,
                  nMpList = en.n_mp_list.Select(x => new MonitorGroupListDto
                  {
                        PointType = x.point_type,
                        PointNumber = x.point_number,
                        PointTypeDesc = x.point_type_desc,
                  }).ToList(),
            })
                .ToArrayAsync();

            return res;
      }

      public async Task<MonitorGroupDto> GetByComponentIdAsync(short componentId)
      {
            
            var res = await context.monitor_group
            .AsNoTracking()
            .OrderBy(x => x.component_id)
            .Where(x => x.component_id == componentId)
            .Select(en => new MonitorGroupDto
            {
                  // Base 
                  ComponentId = en.component_id,
                  Mac = en.hardware_mac,
                  LocationId = en.location_id,
                  IsActive = en.is_active,

                  // Detail
                  Name = en.name,
                  nMpCount = en.n_mp_count,
                  nMpList = en.n_mp_list.Select(x => new MonitorGroupListDto
                  {
                        PointType = x.point_type,
                        PointNumber = x.point_number,
                        PointTypeDesc = x.point_type_desc,
                  }).ToList(),
            })
                .FirstOrDefaultAsync();

            return res;
      }

      public async Task<IEnumerable<MonitorGroupDto>> GetByLocationIdAsync(short locationId)
      {
            var res = await context.monitor_group
            .AsNoTracking()
            .OrderBy(x => x.component_id)
            .Where(x => x.location_id == locationId)
            .Select(en => new MonitorGroupDto
            {
                  // Base 
                  ComponentId = en.component_id,
                  Mac = en.hardware_mac,
                  LocationId = en.location_id,
                  IsActive = en.is_active,

                  // Detail
                  Name = en.name,
                  nMpCount = en.n_mp_count,
                  nMpList = en.n_mp_list.Select(x => new MonitorGroupListDto
                  {
                        PointType = x.point_type,
                        PointNumber = x.point_number,
                        PointTypeDesc = x.point_type_desc,
                  }).ToList(),
            })
                .ToArrayAsync();

            return res;
      }

      public async Task<IEnumerable<MonitorGroupDto>> GetByMacAsync(string mac)
      {
             var res = await context.monitor_group
            .AsNoTracking()
            .OrderBy(x => x.component_id)
            .Where(x => x.mac.Equals(mac))
            .Select(en => new MonitorGroupDto
            {
                  // Base 
                  ComponentId = en.component_id,
                  Mac = en.hardware_mac,
                  LocationId = en.location_id,
                  IsActive = en.is_active,

                  // Detail
                  Name = en.name,
                  nMpCount = en.n_mp_count,
                  nMpList = en.n_mp_list.Select(x => new MonitorGroupListDto
                  {
                        PointType = x.point_type,
                        PointNumber = x.point_number,
                        PointTypeDesc = x.point_type_desc,
                  }).ToList(),
            })
                .ToArrayAsync();

            return res;
      }

      public async Task<IEnumerable<Mode>> GetCommandAsync()
      {
            var dtos = await context.monitor_group_command
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

      public async Task<short> GetLowestUnassignedNumberAsync(int max,string mac)
      {
            if (max <= 0) return -1;

            var query = context.location
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

      public async Task<IEnumerable<Mode>> GetTypeAsync()
      {
            var dtos = await context.monitor_group_type
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

      public async Task<bool> IsAnyByComponentId(short component)
      {
            return await context.location.AnyAsync(x => x.component_id == component);
      }

      public async Task<bool> IsAnyByMacAndComponentIdAsync(string mac, short component)
      {
            return await context.monitor_group.AnyAsync(x => x.mac.Equals(mac) && x.component_id == component);
      }
}
