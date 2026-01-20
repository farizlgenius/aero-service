using System;
using Aero.Application.DTOs;
using Aero.Domain.Entities;

namespace Aero.Application.Interfaces;

public interface IQueryHardwareRepository : IBaseQueryRespository<HardwareDto>
{
      Task<T> GetByMacAsync(string mac);
      Task<bool> IsAnyByMac(string mac);
      Task<short> GetComponentFromMacAsync(string mac);
      Task<string> GetMacFromComponentAsync(short component);
      Task<bool> IsAnyModuleReferenceByMacAsync(string mac);
      Task<ScpSetting> GetScpSettingAsync();

}
