using System;
using Aero.Application.DTOs;
using Aero.Domain.Interfaces;

namespace Aero.Application.Interfaces;

public interface IQCpRepository : IBaseQueryRespository<ControlPointDto>
{
      Task<IEnumerable<ControlPointDto>> GetByMacAsync(string mac);
      Task<short> GetModeNoByOfflineAndRelayModeAsync(short offlineMode,short relayMode);
      Task<int> CountByMacAndUpdateTimeAsync(string mac,DateTime sync);
}
