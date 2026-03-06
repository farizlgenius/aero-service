using System;
using Aero.Application.DTOs;
using Aero.Application.Interfaces;
using Aero.Domain.Entities;
using Aero.Domain.Interfaces;

namespace Aero.Application.Interface;

public interface ICpRepository : IBaseRepository<ControlPointDto,ControlPoint>
{
    Task<IEnumerable<ControlPointDto>> GetByDeviceId(int device);
    Task<short> GetModeNoByOfflineAndRelayModeAsync(short offlineMode, short relayMode);
    Task<int> CountByMacAndUpdateTimeAsync(int device, DateTime sync);
    Task<IEnumerable<ModeDto>> GetOfflineModeAsync();
    Task<IEnumerable<ModeDto>> GetRelayModeAsync();
    Task<IEnumerable<short>> GetAvailableOpAsync(int deviceId, short ModuleId);
    Task<short> GetLowestUnassignedNumberAsync(int max, int device_id);
}
