using System;
using Aero.Application.DTOs;
using Aero.Application.Interfaces;
using Aero.Domain.Entities;
using Aero.Domain.Interfaces;

namespace Aero.Domain.Interface;

public interface IMpRepository : IBaseRepository<MonitorPointDto,MonitorPoint>
{
  Task<IEnumerable<MonitorPointDto>> GetByDeviceId(int device);
      Task<int> SetMaskByIdAsync(int id,bool mask);
    Task<IEnumerable<MonitorPointDto>> GetByDeviceIdAsync(int id);
    Task<int> CountByDeviceIdAndUpdateTimeAsync(int deviceId, DateTime sync);
    Task<IEnumerable<short>> GetAvailableIpAsync(int moduleId);
    Task<bool> IsAnyByDeviceIdAndDriverIdAsync(int deviceId, short driver);
    Task<IEnumerable<ModeDto>> GetInputModeAsync();
    Task<IEnumerable<ModeDto>> GetMonitorPointModeAsync();
    Task<IEnumerable<ModeDto>> GetLogFunctionAsync();
    Task<short> GetLowestUnassignedNumberAsync(int max, int device);
    Task<int> GetDeviceIdFromDriverIdIdAsync(int driver);

}
