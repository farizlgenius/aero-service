using System;
using Aero.Application.DTOs;

namespace Aero.Application.Interfaces;

public interface IQueryModuleRepository : IBaseQueryRespository<ModuleDto>
{
  Task<IEnumerable<ModuleDto>> GetByMacAsync(string mac);
}
