using System;
using Aero.Application.DTOs;
using Aero.Domain.Interfaces;

namespace Aero.Application.Interfaces;

public interface IQTzRepository : IBaseQueryRespository<TimeZoneDto>
{
      Task<int> CountByLocationIdAndUpdateTimeAsync(short locationId,DateTime sync);
      Task<IEnumerable<Mode>> GetCommandAsync();
      Task<IEnumerable<Mode>> GetModeAsync();
}
