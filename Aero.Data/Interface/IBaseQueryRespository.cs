using System;

namespace Aero.Domain.Interfaces;

public interface IBaseQueryRespository<T>
{
      Task<bool> IsAnyByComponentId(short component);
      Task<IEnumerable<T>> GetAsync();
      Task<T> GetByComponentIdAsync(short componentId);
      Task<IEnumerable<T>> GetByLocationIdAsync(short locationId);
      Task<short> GetLowestUnassignedNumberAsync(int max);

}
