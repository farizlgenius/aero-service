using System;
using Aero.Application.DTOs;
using Aero.Domain.Interfaces;

namespace Aero.Application.Interfaces;

public interface IQDoorRepository : IBaseQueryRespository<DoorDto>
{
      Task<IEnumerable<DoorDto>> GetByMacAsync(string mac);
      Task<short> GetLowestUnassignedRexNumberAsync();
      Task<int> CountByMacAndUpdateTimeAsync(string mac ,DateTime sync);
      Task<IEnumerable<ModeDto>> GetReaderModeAsync();
      Task<IEnumerable<ModeDto>> GetStrikeModeAsync();
      Task<IEnumerable<ModeDto>> GetDoorModeAsync();
      Task<IEnumerable<ModeDto>> GetApbModeAsync();
      Task<IEnumerable<short>> GetAvailableReaderFromMacAndComponentIdAsync(string mac,short component);
      Task<IEnumerable<ModeDto>> GetReaderOutModeAsync();
      Task<short> GetLowestUnassignedNumberByMacAsync(string mac,int max);
      Task<short> GetLowestUnassignedReaderNumberNoLimitAsync();
      Task<short> GetLowestUnassignedSensorNumberNoLimitAsync();
      Task<short> GetLowestUnassignedStrikeNumberNoLimitAsync();

}
