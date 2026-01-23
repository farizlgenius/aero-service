using System;
using Aero.Application.DTOs;
using Aero.Domain.Interfaces;

namespace Aero.Application.Interfaces;

public interface IQModuleRepository : IBaseQueryRespository<ModuleDto>
{
      Task<int> CountByMacAndUpdateTimeAsync(string mac,DateTime sync);
      Task<IEnumerable<ModuleDto>> GetByMacAsync(string mac);
      Task<bool> IsAnyByComponentAndMacAsnyc(string mac,short component);
      Task<IEnumerable<ModeDto>> GetBaudrateAsync();
      Task<IEnumerable<ModeDto>> GetProtocolAsync();
}
