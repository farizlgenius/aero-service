using System;
using Aero.Application.DTOs;
using Aero.Domain.Interfaces;

namespace Aero.Application.Interfaces;

public interface IQActionRepository : IBaseQueryRespository<ActionDto>
{
      Task<int> CountByMacAndUpdateTimeAsync(string mac,DateTime sync);
}
