using System;
using Aero.Application.DTOs;
using Aero.Domain.Interfaces;

namespace Aero.Application.Interfaces;

public interface IQTzRepository : IBaseQueryRespository<TimeZoneDto>
{
      Task<int> CountByLocationIdAndUpdateTimeAsync(short locationId,DateTime sync);
      Task<IEnumerable<ModeDto>> GetCommandAsync();
      Task<IEnumerable<ModeDto>> GetModeAsync();
}
