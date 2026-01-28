using System;
using Aero.Domain.Entities;
using Aero.Domain.Interfaces;

namespace Aero.Domain.Interface;

public interface IAlvlRepository : IBaseRepository<AccessLevel>
{
      Task<int> AddCreateAsync(CreateUpdateAccessLevel domain);
      Task<int> UpdateCreateAsync(CreateUpdateAccessLevel domain);
}
