using System;
using Aero.Application.DTOs;
using Aero.Application.Interfaces;
using Aero.Domain.Entities;
using Aero.Domain.Interfaces;

namespace Aero.Application.Interface;

public interface IDoorRepository : IBaseRepository<DoorDto,Door>
{
      Task<int> ChangeDoorModeAsync(int deviceId,short driverid,short mode);
    Task<IEnumerable<DoorDto>> GetByDeviceIdAsync(int id);
    // Task<short> GetLowestUnassignedRexNumberAsync();
    Task<short> GetLowestUnassignedNumberByDeviceIdAsync(int max,int device);
    Task<int> CountByDeviceIdAndUpdateTimeAsync(int device, DateTime sync);
    Task<IEnumerable<ModeDto>> GetReaderModeAsync();
    Task<IEnumerable<ModeDto>> GetStrikeModeAsync();
    Task<IEnumerable<ModeDto>> GetDoorModeAsync();
    Task<IEnumerable<ModeDto>> GetApbModeAsync();
    Task<IEnumerable<short>> GetAvailableReaderFromDeviceIdAndDriverIdAsync(int device, int driver);
    Task<IEnumerable<ModeDto>> GetReaderOutModeAsync();
    // Task<short> GetLowestUnassignedNumberByMacAsync(string mac, int max);
    // Task<short> GetLowestUnassignedReaderNumberNoLimitAsync();
    // Task<short> GetLowestUnassignedSensorNumberNoLimitAsync();
    // Task<short> GetLowestUnassignedStrikeNumberNoLimitAsync();
    Task<IEnumerable<ModeDto>> GetDoorAccessControlFlagAsync();
    Task<IEnumerable<ModeDto>> GetDoorSpareFlagAsync();
    Task<IEnumerable<ModeDto>> GetOsdpBaudrateAsync();
    Task<IEnumerable<ModeDto>> GetOsdpAddressAsync();
}
