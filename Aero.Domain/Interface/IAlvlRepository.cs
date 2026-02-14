using System;
using Aero.Domain.Entities;
using Aero.Domain.Interfaces;

namespace Aero.Domain.Interface;

public interface IAlvlRepository : IBaseRepository<AccessLevel>
{
      Task<int> AddCreateAsync(AccessLevel domain);
      Task<int> UpdateCreateAsync(AccessLevel domain);
}
