using System;
using Aero.Application.DTOs;
using Aero.Domain.Interfaces;

namespace Aero.Application.Interfaces;

public interface IQDoorRepository : IBaseQueryRespository<DoorDto>
{
      Task<IEnumerable<DoorDto>> GetByMacAsync(string mac);
      Task<short> GetLowestUnassignedRexNumberAsync();
      Task<int> CountByMacAndUpdateTimeAsync(string mac ,DateTime sync);
      Task<IEnumerable<Mode>> GetReaderModeAsync();
      Task<IEnumerable<Mode>> GetStrikeModeAsync();
      Task<IEnumerable<Mode>> GetDoorModeAsync();
      Task<IEnumerable<Mode>> GetApbModeAsync();
      Task<IEnumerable<short>> GetAvailableReaderFromMacAndComponentIdAsync(string mac,short component);
      Task<IEnumerable<Mode>> GetReaderOutModeAsync();
      Task<short> GetLowestUnassignedNumberByMacAsync(string mac,int max);
      Task<short> GetLowestUnassignedReaderNumberNoLimitAsync();
      Task<short> GetLowestUnassignedSensorNumberNoLimitAsync();
      Task<short> GetLowestUnassignedStrikeNumberNoLimitAsync();
      Task<IEnumerable<Mode>> GetDoorAccessControlFlagAsync();
      Task<IEnumerable<Mode>> GetDoorSpareFlagAsync();
      Task<IEnumerable<Mode>> GetOsdpBaudrateAsync();
      Task<IEnumerable<Mode>> GetOsdpAddressAsync();

}
