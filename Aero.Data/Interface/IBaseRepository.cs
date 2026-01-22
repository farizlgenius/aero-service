using System;

namespace Aero.Domain.Interfaces;

public interface IBaseRepository<T>
{
      Task<int> AddAsync(T data);
      Task<int> DeleteByComponentIdAsync(short component);
      Task<int> UpdateAsync(T newData);
}
