using System;
using Aero.Domain.Entities;
using Aero.Domain.Interfaces;

namespace Aero.Application.Interfaces;

public interface ICredRepository : IBaseRepository<Credential>
{
      Task<int> DeleteByCardNoAsync(long Cardno);
}
