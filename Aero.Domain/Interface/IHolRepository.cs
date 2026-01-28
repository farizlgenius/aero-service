using System;
using Aero.Domain.Entities;
using Aero.Domain.Interfaces;

namespace Aero.Domain.Interface;

public interface IHolRepository : IBaseRepository<Holiday>
{
      Task<int> RemoveAllAsync();
      Task<Aero.Domain.Entities.Holiday> GetByComponentIdAsync(short component);
}
