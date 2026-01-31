using System;
using Aero.Domain.Entities;
using Aero.Domain.Interface;

namespace Aero.Infrastructure.Repositories;

public class TriggerRepository : ITriggerRepository
{
      public Task<int> AddAsync(Trigger data)
      {
            throw new NotImplementedException();
      }

      public Task<int> DeleteByComponentIdAsync(short component)
      {
            throw new NotImplementedException();
      }

      public Task<int> UpdateAsync(Trigger newData)
      {
            throw new NotImplementedException();
      }
}
