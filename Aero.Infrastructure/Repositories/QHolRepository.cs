using System;
using Aero.Application.DTOs;
using Aero.Application.Interfaces;
using Aero.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Aero.Infrastructure.Repositories;

public sealed class QHolRepository(AppDbContext context) : IQHolRepository
{
      public async Task<int> CountByLocationIdAndUpdateTimeAsync(short locationId, DateTime sync)
      {
            var res = await context.holiday
            .AsNoTracking()
            .Where(x => x.location_id == locationId && x.updated_date < sync)
            .CountAsync();

            return res;
      }

      public Task<IEnumerable<HolidayDto>> GetAsync()
      {
            throw new NotImplementedException();
      }

      public Task<HolidayDto> GetByComponentIdAsync(short componentId)
      {
            throw new NotImplementedException();
      }

      public Task<IEnumerable<HolidayDto>> GetByLocationIdAsync(short locationId)
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
