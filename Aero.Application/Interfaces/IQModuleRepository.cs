using System;
using Aero.Application.DTOs;
using Aero.Domain.Entities;
using Aero.Domain.Interfaces;

namespace Aero.Application.Interfaces;

public interface IQModuleRepository : IBaseQueryRespository<ModuleDto>
{
      Task<int> CountByMacAndUpdateTimeAsync(string mac,DateTime sync);
      Task<IEnumerable<ModuleDto>> GetByMacAsync(string mac);
      Task<bool> IsAnyByComponentAndMacAsnyc(string mac,short component);
      Task<IEnumerable<Mode>> GetBaudrateAsync();
      Task<IEnumerable<Mode>> GetProtocolAsync();
}
