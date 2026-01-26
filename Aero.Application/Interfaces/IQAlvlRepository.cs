using System;
using Aero.Application.DTOs;
using Aero.Domain.Interfaces;

namespace Aero.Application.Interfaces;

public interface IQAlvlRepository : IBaseQueryRespository<AccessLevelDto>
{
      Task<int> CountByLocationIdAndUpdateTimeAsync(short locationId,DateTime sync);
      Task<string> GetACRNameByComponentIdAndMacAsync(short component,string mac);
      Task<string> GetTimezoneNameByComponentIdAsync(short component);
      Task<List<string>> GetUniqueMacFromDoorIdAsync(short doorId);
}
