using System;
using Aero.Application.DTOs;
using Aero.Application.Interfaces;
using Aero.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Aero.Infrastructure.Repositories;

public class QIntervalRepository(AppDbContext context) : IQIntervalRepository
{
      public async Task<int> CountByLocationIdAndUpdateTimeAsync(short locationId, DateTime sync)
      {
            var res = await context.interval
            .AsNoTracking()
            .Where(x => x.location_id == locationId && x.updated_date > sync)
            .CountAsync();

            return res;
      }

      public Task<IEnumerable<IntervalDto>> GetAsync()
      {
            throw new NotImplementedException();
      }

      public Task<IntervalDto> GetByComponentIdAsync(short componentId)
      {
            throw new NotImplementedException();
      }

      public Task<IEnumerable<IntervalDto>> GetByLocationIdAsync(short locationId)
      {
            throw new NotImplementedException();
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
