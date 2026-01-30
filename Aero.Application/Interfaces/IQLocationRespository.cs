using System;
using Aero.Application.DTOs;
using Aero.Domain.Interfaces;

namespace Aero.Application.Interfaces;

public interface IQLocationRespository : IBaseQueryRespository<LocationDto>
{

      Task<IEnumerable<LocationDto>> GetLocationsByListIdAsync(LocationRangeDto dto);
      Task<bool> IsAnyByLocationNameAsync(string name);
      Task<List<string>> CheckRelateReferenceByComponentIdAsync(short component);
}
