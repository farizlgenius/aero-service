using System;
using Aero.Application.DTOs;
using Aero.Domain.Interfaces;

namespace Aero.Application.Interfaces;

public interface IQDoorRepository : IBaseQueryRespository<DoorDto>
{
      Task<IEnumerable<DoorDto>> GetByMacAsync(string mac);
      Task<short> GetLowestUnassignedRexNumberAsync();
      Task<int> CountByMacAndUpdateTimeAsync(string mac ,DateTime sync);
}
