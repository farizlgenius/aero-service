using System;
using Aero.Application.DTOs;
using Aero.Domain.Interfaces;

namespace Aero.Application.Interfaces;

public interface IQHolRepository : IBaseQueryRespository<HolidayDto>
{
      Task<int> CountByLocationIdAndUpdateTimeAsync(short locationId,DateTime sync);
      Task<bool> IsAnyWithSameDataAsync(short day,short month,short year);
}
