using System;
using Aero.Application.DTOs;
using Aero.Application.Interfaces;
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
            throw new NotImplementedException();
      }
      

      public Task<MonitorGroupDto> GetByComponentIdAsync(short componentId)
      {
            throw new NotImplementedException();
      }

      public Task<IEnumerable<MonitorGroupDto>> GetByLocationIdAsync(short locationId)
      {
            throw new NotImplementedException();
      }

      public async Task<IEnumerable<MonitorGroupDto>> GetByMacAsync(string mac)
      {
            var res = await context.monitor_group
            .AsNoTracking()
            .OrderBy(x => x.component_id)
            .Where(x => x.hardware_mac.Equals(mac))
            .Select(en => new MonitorGroupDto
                {
                    // Base 
                    Uuid = en.uuid,
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

      public Task<short> GetLowestUnassignedNumberAsync(int max)
      {
            throw new NotImplementedException();
      }

      public Task<bool> IsAnyByComponet(short component)
      {
            throw new NotImplementedException();
      }
}
