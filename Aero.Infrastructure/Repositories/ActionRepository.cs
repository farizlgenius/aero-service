using System;
using Aero.Application.DTOs;
using Aero.Application.Interfaces;
using Aero.Domain.Entities;
using Aero.Infrastructure.Persistences;
using Microsoft.EntityFrameworkCore;

namespace Aero.Infrastructure.Repositories;

public sealed class ActionRepository(AppDbContext context) : IActionRepository
{
    public Task<int> AddAsync(Domain.Entities.Action data)
    {
        throw new NotImplementedException();
    }

    public async Task<int> CountByMacAndUpdateTimeAsync(string mac, DateTime sync)
      {
            var res = await context.action
            .AsNoTracking()
            .Where(x => x.procedure.trigger.mac.Equals(mac) && x.updated_date > sync)
            .CountAsync();

            return res;
      }

    public Task<int> DeleteByIdAsync(short id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<ActionDto>> GetAsync()
    {
        throw new NotImplementedException();
    }

    public Task<ActionDto> GetByIdAsync(short id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<ActionDto>> GetByLocationIdAsync(short locationId)
    {
        throw new NotImplementedException();
    }

    public Task<short> GetLowestUnassignedNumberAsync(int max, string mac)
    {
        throw new NotImplementedException();
    }

    public Task<Pagination<ActionDto>> GetPaginationAsync(PaginationParamsWithFilter param, short location)
    {
        throw new NotImplementedException();
    }

    public Task<bool> IsAnyById(short id)
    {
        throw new NotImplementedException();
    }

    public Task<int> UpdateAsync(Domain.Entities.Action newData)
    {
        throw new NotImplementedException();
    }
}
