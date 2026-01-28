using System;
using Aero.Application.DTOs;
using Aero.Domain.Interfaces;

namespace Aero.Application.Interfaces;

public interface IQAreaRepository : IBaseQueryRespository<AccessAreaDto>
{
      Task<int> CountByLocationIdAndUpdateTimeAsync(short locationId,DateTime sync);
      Task<IEnumerable<ModeDto>> GetCommandAsync();
      Task<IEnumerable<ModeDto>> GetAccessControlOptionAsync();
      Task<IEnumerable<ModeDto>> GetOccupancyControlOptionAsync();
      Task<IEnumerable<ModeDto>> GetAreaFlagOptionAsync();
      Task<IEnumerable<ModeDto>> GetMultiOccupancyOptionAsync();
}
