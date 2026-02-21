using System;
using Aero.Application.DTOs;
using Aero.Application.Interfaces;
using Aero.Domain.Entities;
using Aero.Domain.Interfaces;

namespace Aero.Application.Interface;

public interface ICpRepository : IBaseRepository<ControlPointDto,ControlPoint>
{
    Task<IEnumerable<ControlPointDto>> GetByMacAsync(string mac);
    Task<short> GetModeNoByOfflineAndRelayModeAsync(short offlineMode, short relayMode);
    Task<int> CountByMacAndUpdateTimeAsync(string mac, DateTime sync);
    Task<IEnumerable<Mode>> GetOfflineModeAsync();
    Task<IEnumerable<Mode>> GetRelayModeAsync();
    Task<IEnumerable<short>> GetAvailableOpAsync(string mac, short ModuleId);
    Task<ControlPointDto> GetByMacAndComponentIdAsync(string mac, short component);
}
