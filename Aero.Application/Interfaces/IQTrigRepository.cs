using System;
using Aero.Application.DTOs;
using Aero.Domain.Interfaces;

namespace Aero.Application.Interfaces;

public interface IQTrigRepository : IBaseQueryRespository<TriggerDto>
{
      Task<int> CountByMacAndUpdateTimeAsync(string mac,DateTime sync);
}
