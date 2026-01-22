using System;
using Aero.Application.DTOs;
using Aero.Domain.Interfaces;

namespace Aero.Application.Interfaces;

public interface IQMpgRepository : IBaseQueryRespository<MonitorGroupDto>
{
      Task<IEnumerable<MonitorGroupDto>> GetByMacAsync(string mac);
      Task<int> CountByMacAndUpdateTimeAsync(string mac, DateTime sync);
}
