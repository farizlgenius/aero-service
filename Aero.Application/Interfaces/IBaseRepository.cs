using Aero.Domain.Entities;
using System;

namespace Aero.Application.Interfaces;

public interface IBaseRepository<Y,T> 
{
      Task<int> AddAsync(T data);
      Task<int> DeleteByIdAsync(int id);
      Task<int> UpdateAsync(T newData);
    Task<bool> IsAnyById(int id);
    Task<IEnumerable<Y>> GetAsync();
    Task<Y> GetByIdAsync(int id);
    Task<IEnumerable<Y>> GetByLocationIdAsync(int locationId);
    Task<Pagination<Y>> GetPaginationAsync(PaginationParamsWithFilter param, int location);
    Task<bool> IsAnyByNameAsync(string name);
    

}
