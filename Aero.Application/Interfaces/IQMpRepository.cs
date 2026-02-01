using System;
using Aero.Application.DTOs;
using Aero.Domain.Entities;
using Aero.Domain.Interfaces;

namespace Aero.Application.Interfaces;

public interface IQMpRepository : IBaseQueryRespository<MonitorPointDto>
{
      Task<IEnumerable<MonitorPointDto>> GetByMacAsync(string mac);
      Task<int> CountByMacAndUpdateTimeAsync(string mac,DateTime sync);
      Task<IEnumerable<short>> GetAvailableIpAsync(string mac, short sio);
      Task<bool> IsAnyByMacAndComponentIdAsync(string mac,short component);
      Task<string> GetMacFromComponentIdAsync(short component);
      Task<IEnumerable<Mode>> GetInputModeAsync();
      Task<IEnumerable<Mode>> GetMonitorPointModeAsync();
      Task<IEnumerable<Mode>> GetLogFunctionAsync();

}
