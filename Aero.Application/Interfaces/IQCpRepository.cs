using System;
using Aero.Application.DTOs;
using Aero.Domain.Interfaces;

namespace Aero.Application.Interfaces;

public interface IQCpRepository : IBaseQueryRespository<ControlPointDto>
{
      Task<IEnumerable<ControlPointDto>> GetByMacAsync(string mac);
      Task<short> GetModeNoByOfflineAndRelayModeAsync(short offlineMode,short relayMode);
      Task<int> CountByMacAndUpdateTimeAsync(string mac,DateTime sync);
      Task<IEnumerable<ModeDto>> GetOfflineModeAsync();
      Task<IEnumerable<ModeDto>> GetRelayModeAsync();
      Task<IEnumerable<short>> GetAvailableOpAsync(string mac, short ModuleId);
      Task<ControlPointDto> GetByMacAndComponentIdAsync(string mac,short component);
}
