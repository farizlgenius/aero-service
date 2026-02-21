using Aero.Domain.Entities;
using System;

namespace Aero.Application.Interfaces;

public interface IBaseRepository<Y,T>
{
      Task<int> AddAsync(T data);
      Task<int> DeleteByIdAsync(short id);
      Task<int> UpdateAsync(T newData);
    Task<bool> IsAnyById(short id);
    Task<IEnumerable<Y>> GetAsync();
    Task<Y> GetByIdAsync(short id);
    Task<IEnumerable<Y>> GetByLocationIdAsync(short locationId);
    Task<short> GetLowestUnassignedNumberAsync(int max, string mac);
    Task<Pagination<Y>> GetPaginationAsync(PaginationParamsWithFilter param, short location);

}
