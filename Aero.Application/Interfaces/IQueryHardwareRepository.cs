using System;
using Aero.Application.DTOs;

namespace Aero.Application.Interfaces;

public interface IQueryHardwareRepository : IBaseQueryRespository<HardwareDto>
{
      
      Task<short> GetComponentFromMacAsync(string mac);
      Task<string> GetMacFromComponentAsync(short component);
      Task<bool> IsAnyModuleReferenceByMacAsync(string mac);
      
}
