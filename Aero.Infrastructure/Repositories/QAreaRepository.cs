using System;
using Aero.Application.DTOs;
using Aero.Application.Interfaces;
using Aero.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Aero.Infrastructure.Repositories;

public class QAreaRepository(AppDbContext context) : IQAreaRepository
{
      public async Task<int> CountByLocationIdAndUpdateTimeAsync(short locationId, DateTime sync)
      {
            var res = await context.area
            .AsNoTracking()
            .Where(x => x.location_id == locationId && x.updated_date > sync)
            .CountAsync();

            return res;
      }

      public async Task<IEnumerable<ModeDto>> GetAccessControlOptionAsync()
      {
            var dto = await context.area_access_control
                .Select(x => new ModeDto
                {
                    Name = x.name,
                    Value = x.value,
                    Description = x.description
                })
                .ToArrayAsync();

            return dto;
      }

      public async Task<IEnumerable<ModeDto>> GetAreaFlagOptionAsync()
      {
              var dto = await context.area_flag
                .Select(x => new ModeDto
                {
                    Name = x.name,
                    Value = x.value,
                    Description = x.description
                })
                .ToArrayAsync();

                return dto;
      }

      public async Task<IEnumerable<AccessAreaDto>> GetAsync()
      {
            var res = await context.area
            .AsNoTracking()
            .Select(entity => new AccessAreaDto
            {
                  // Base
                  Uuid = entity.uuid,
                  LocationId = entity.location_id,
                  IsActive = entity.is_active,
                  component_id = entity.component_id,

                  // extend_desc
                  Name = entity.name,
                  MultiOccupancy = entity.multi_occ,
                  AccessControl = entity.access_control,
                  OccControl = entity.occ_control,
                  OccSet = entity.occ_set,
                  OccMax = entity.occ_max,
                  OccDown = entity.occ_down,
                  OccUp = entity.occ_up,
                  AreaFlag = entity.area_flag,
            })
            .ToArrayAsync();

            return res;
      }

      public async Task<AccessAreaDto> GetByComponentIdAsync(short componentId)
      {
            var res = await context.area
            .AsNoTracking()
            .Where(x => x.component_id == componentId)
            .OrderBy(x => x.component_id)
            .Select(entity => new AccessAreaDto
            {
                  // Base
                  Uuid = entity.uuid,
                  LocationId = entity.location_id,
                  IsActive = entity.is_active,
                  component_id = entity.component_id,

                  // extend_desc
                  Name = entity.name,
                  MultiOccupancy = entity.multi_occ,
                  AccessControl = entity.access_control,
                  OccControl = entity.occ_control,
                  OccSet = entity.occ_set,
                  OccMax = entity.occ_max,
                  OccDown = entity.occ_down,
                  OccUp = entity.occ_up,
                  AreaFlag = entity.area_flag,
            })
            .FirstOrDefaultAsync();

            return res;
      }

      public async Task<IEnumerable<AccessAreaDto>> GetByLocationIdAsync(short locationId)
      {
            var res = await context.area
            .AsNoTracking()
            .Where(x => x.location_id == locationId)
            .OrderBy(x => x.component_id)
            .Select(entity => new AccessAreaDto
            {
                  // Base
                  Uuid = entity.uuid,
                  LocationId = entity.location_id,
                  IsActive = entity.is_active,
                  component_id = entity.component_id,

                  // extend_desc
                  Name = entity.name,
                  MultiOccupancy = entity.multi_occ,
                  AccessControl = entity.access_control,
                  OccControl = entity.occ_control,
                  OccSet = entity.occ_set,
                  OccMax = entity.occ_max,
                  OccDown = entity.occ_down,
                  OccUp = entity.occ_up,
                  AreaFlag = entity.area_flag,
            })
            .ToArrayAsync();

            return res;
      }

      public async Task<IEnumerable<ModeDto>> GetCommandAsync()
      {
            var dto = await context.access_area_command
                .Select(x => new ModeDto 
                {
                    Name = x.name,
                    Value = x.value,
                    Description = x.description
                })
                .ToArrayAsync();

            return dto;
      }

      public async Task<short> GetLowestUnassignedNumberAsync(int max)
      {
            if (max <= 0) return -1;

            var query = context.area
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

      public async Task<IEnumerable<ModeDto>> GetMultiOccupancyOptionAsync()
      {
             var dto = await context.multi_occupancy
                .Select(x => new ModeDto
                {
                    Name = x.name,
                    Value = x.value,
                    Description = x.description
                })
                .ToArrayAsync();

                return dto;
      }

      public async Task<IEnumerable<ModeDto>> GetOccupancyControlOptionAsync()
      {
             var dto = await context.occupancy_control
                .Select(x => new ModeDto
                {
                    Name = x.name,
                    Value = x.value,
                    Description = x.description
                })
                .ToArrayAsync();

                return dto;

      }

      public async Task<bool> IsAnyByComponentId(short component)
      {
            return await context.area.AnyAsync(x => x.component_id == component);
      }
}
