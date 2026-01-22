using System;
using Aero.Application.DTOs;
using Aero.Domain.Interfaces;

namespace Aero.Application.Interfaces;

public interface IQMpRepository : IBaseQueryRespository<MonitorPointDto>
{
      Task<IEnumerable<MonitorPointDto>> GetByMacAsync(string mac);
      Task<int> CountByMacAndUpdateTimeAsync(string mac,DateTime sync);
}
