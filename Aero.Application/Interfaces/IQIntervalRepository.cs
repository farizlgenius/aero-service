using System;
using Aero.Application.DTOs;
using Aero.Domain.Interfaces;

namespace Aero.Application.Interfaces;

public interface IQIntervalRepository : IBaseQueryRespository<IntervalDto>
{
      Task<bool> IsAnyOnEachDays(IntervalDto dto);
      Task<bool> IsAnyReferenceByComponentAsync(short component);
      Task<IEnumerable<short>> GetTimezoneIntervalIdByIntervalComponentIdAsync(short component);
      Task<IEnumerable<IntervalDto>> GetIntervalFromTimezoneComponentIdAsync(short component); 
      Task<int> CountByLocationIdAndUpdateTimeAsync(short locationId, DateTime sync)

}
