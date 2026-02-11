using System;
using Aero.Application.DTOs;
using Aero.Application.Interfaces;
using Aero.Domain.Entities;
using Aero.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Aero.Infrastructure.Repositories;

public sealed class QActionRepository(AppDbContext context) : IQActionRepository
{
      public async Task<int> CountByMacAndUpdateTimeAsync(string mac, DateTime sync)
      {
            var res = await context.action
            .AsNoTracking()
            .Where(x => x.procedure.trigger.hardware_mac.Equals(mac) && x.updated_date > sync)
            .CountAsync();

            return res;
      }

      public Task<IEnumerable<ActionDto>> GetAsync()
      {
            throw new NotImplementedException();
      }

      public Task<ActionDto> GetByComponentIdAsync(short componentId)
      {
            throw new NotImplementedException();
      }

      public Task<IEnumerable<ActionDto>> GetByLocationIdAsync(short locationId)
      {
            throw new NotImplementedException();
      }

      public Task<short> GetLowestUnassignedNumberAsync(int max,string mac)
      {
            throw new NotImplementedException();
      }

    public Task<Pagination<ActionDto>> GetPaginationAsync(PaginationParamsWithFilter param,short location)
    {
        throw new NotImplementedException();
    }

    public Task<bool> IsAnyByComponentId(short component)
      {
            throw new NotImplementedException();
      }
}
